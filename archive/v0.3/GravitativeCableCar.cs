using CitiesHarmony.API;
using HarmonyLib;
using ICities;
using System;
using System.Reflection;
using UnityEngine;
using ColossalFramework;

namespace GravitativeCableCar
{
    public class Mod : IUserMod
    {

        public string Name => "Gravitative Cable Car";
        public string Description => "make cable cars subject to gravity v0.3";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());

            TopSwayPosition = new Vector3(0.0f, 0.0f, 0.0f);

        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }

        public static Vector3 TopSwayPosition;

    }

    public static class Patcher
    {
        private const string HarmonyId = "sway.GravitativeCableCar";

        private static bool patched = false;

        public static void PatchAll()
        {
            if (patched) return;

            UnityEngine.Debug.Log("Gravitative Cable Car: Patching...");

            patched = true;


            // Harmony.DEBUG = true;
            var harmony = new Harmony("sway.GravitativeCableCar");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static void UnpatchAll()
        {
            if (!patched) return;

            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);

            patched = false;

            UnityEngine.Debug.Log("Gravitative Cable Car: Reverted...");
        }

    }

    // Modified from Vehicle's RenderInstance method
    [HarmonyPatch(typeof(Vehicle), "RenderInstance", new Type[] { typeof(RenderManager.CameraInfo), typeof(VehicleInfo), typeof(Vector3), typeof(Quaternion), typeof(Vector3), typeof(Vector4), typeof(Vector4), typeof(Vector3), typeof(float), typeof(Color), typeof(Vehicle.Flags), typeof(int), typeof(InstanceID), typeof(bool), typeof(bool) })]
    public static class VehicleRenderInstancePatch
    {
        // Run before the original method. Skip the original one if the vehicle is a cable car
        public static bool Prefix(RenderManager.CameraInfo cameraInfo, VehicleInfo info, Vector3 position, ref Quaternion rotation, Vector3 swayPosition, Vector4 lightState, Vector4 tyrePosition, Vector3 velocity, float acceleration, Color color, Vehicle.Flags flags, int variationMask, InstanceID id, bool underground, bool overground)
        {
            // if the vehicle is not a cable car, skip and use the original method
            if ((int)info.m_vehicleType != 0x1000){
                return true;
            }
            
            // limit rotation along the x and z axes for all meshes except submesh1
            // submesh1 would rotate in the original way(along with the cable)
            Quaternion originalRotation = rotation;
            rotation.x = 0.0f;
            rotation.z = 0.0f;

            // change how cable cars sway
            // so they don't move up and down on the cables as if they're ships moving in sea waves
            swayPosition.y = 0.0f;


            if ((cameraInfo.m_layerMask & (1 << info.m_prefabDataLayer)) == 0)
            {
                // return false to skip the original method
                return false;
            }
            Vector3 scale = Vector3.one;
            if ((flags & Vehicle.Flags.Inverted) != 0)
            {
                scale = new Vector3(-1f, 1f, -1f);
                Vector4 vector = lightState;
                lightState.x = vector.y;
                lightState.y = vector.x;
                lightState.z = vector.w;
                lightState.w = vector.z;
            }

            info.m_vehicleAI.RenderExtraStuff(id.Vehicle, ref Singleton<VehicleManager>.instance.m_vehicles.m_buffer[id.Vehicle], cameraInfo, id, position, rotation, tyrePosition, lightState, scale, swayPosition, underground, overground);
            
            if (cameraInfo.CheckRenderDistance(position, info.m_lodRenderDistance))
            {
                VehicleManager instance = Singleton<VehicleManager>.instance;
                Matrix4x4 bodyMatrix = info.m_vehicleAI.CalculateBodyMatrix(flags, ref position, ref rotation, ref scale, ref swayPosition);
                Matrix4x4 topBodyMatrix = info.m_vehicleAI.CalculateBodyMatrix(flags, ref position, ref originalRotation, ref scale, ref Mod.TopSwayPosition);
                Matrix4x4 value = info.m_vehicleAI.CalculateTyreMatrix(flags, ref position, ref rotation, ref scale, ref bodyMatrix);
                if (Singleton<InfoManager>.instance.CurrentMode == InfoManager.InfoMode.None)
                {
                    RenderGroup.MeshData effectMeshData = info.m_vehicleAI.GetEffectMeshData();
                    EffectInfo.SpawnArea area = new EffectInfo.SpawnArea(bodyMatrix, effectMeshData, info.m_generatedInfo.m_tyres, info.m_lightPositions);
                    if (info.m_effects != null)
                    {
                        for (int i = 0; i < info.m_effects.Length; i++)
                        {
                            VehicleInfo.Effect effect = info.m_effects[i];
                            if (((effect.m_vehicleFlagsRequired | effect.m_vehicleFlagsForbidden) & flags) == effect.m_vehicleFlagsRequired && effect.m_parkedFlagsRequired == VehicleParked.Flags.None)
                            {
                                effect.m_effect.RenderEffect(id, area, velocity, acceleration, 1f, -1f, Singleton<SimulationManager>.instance.m_simulationTimeDelta, cameraInfo);
                            }
                        }
                    }
                }
                if ((flags & Vehicle.Flags.Inverted) != 0)
                {
                    tyrePosition.x = 0f - tyrePosition.x;
                    tyrePosition.y = 0f - tyrePosition.y;
                }
                MaterialPropertyBlock materialBlock = instance.m_materialBlock;
                materialBlock.Clear();
                materialBlock.SetMatrix(instance.ID_TyreMatrix, value);
                materialBlock.SetVector(instance.ID_TyrePosition, tyrePosition);
                materialBlock.SetVector(instance.ID_LightState, lightState);
                bool flag = Singleton<ToolManager>.instance.m_properties.m_mode == ItemClass.Availability.AssetEditor;
                if (!flag)
                {
                    materialBlock.SetColor(instance.ID_Color, color);
                }
                bool flag2 = true;
                if (flag)
                {
                    flag2 = BuildingDecoration.IsMainMeshRendered();
                }
                if (info.m_subMeshes != null)
                {
                    for (int j = 0; j < info.m_subMeshes.Length; j++)
                    {
                        VehicleInfo.MeshInfo meshInfo = info.m_subMeshes[j];
                        VehicleInfoBase subInfo = meshInfo.m_subInfo;
                        if ((!flag && ((meshInfo.m_vehicleFlagsRequired | meshInfo.m_vehicleFlagsForbidden) & flags) == meshInfo.m_vehicleFlagsRequired && (meshInfo.m_variationMask & variationMask) == 0 && meshInfo.m_parkedFlagsRequired == VehicleParked.Flags.None) || (flag && BuildingDecoration.IsSubMeshRendered(subInfo)))
                        {
                            if (!(subInfo != null))
                            {
                                continue;
                            }
                            instance.m_drawCallData.m_defaultCalls++;
                            if (underground)
                            {
                                if (subInfo.m_undergroundMaterial == null && subInfo.m_material != null)
                                {
                                    VehicleProperties properties = instance.m_properties;
                                    if (properties != null)
                                    {
                                        subInfo.m_undergroundMaterial = new Material(properties.m_undergroundShader);
                                        subInfo.m_undergroundMaterial.CopyPropertiesFromMaterial(subInfo.m_material);
                                    }
                                }
                                subInfo.m_undergroundMaterial.SetVectorArray(instance.ID_TyreLocation, subInfo.m_generatedInfo.m_tyres);
                                if (j == 1)
                                {
                                    Graphics.DrawMesh(subInfo.m_mesh, topBodyMatrix, subInfo.m_undergroundMaterial, instance.m_undergroundLayer, null, 0, materialBlock);
                                }
                                else
                                {
                                    Graphics.DrawMesh(subInfo.m_mesh, bodyMatrix, subInfo.m_undergroundMaterial, instance.m_undergroundLayer, null, 0, materialBlock);
                                }
                            }
                            if (overground)
                            {
                                subInfo.m_material.SetVectorArray(instance.ID_TyreLocation, subInfo.m_generatedInfo.m_tyres);
                                if (j == 1)
                                {
                                    Graphics.DrawMesh(subInfo.m_mesh, topBodyMatrix, subInfo.m_material, info.m_prefabDataLayer, null, 0, materialBlock);
                                }
                                else
                                {
                                    Graphics.DrawMesh(subInfo.m_mesh, bodyMatrix, subInfo.m_material, info.m_prefabDataLayer, null, 0, materialBlock);
                                }
                            }
                        }
                        else if (subInfo == null)
                        {
                            flag2 = false;
                        }
                    }
                }
                if (!flag2)
                {
                    // return false to skip the original method
                    return false;
                }
                instance.m_drawCallData.m_defaultCalls++;
                if (underground)
                {
                    if (info.m_undergroundMaterial == null && info.m_material != null)
                    {
                        VehicleProperties properties2 = instance.m_properties;
                        if (properties2 != null)
                        {
                            info.m_undergroundMaterial = new Material(properties2.m_undergroundShader);
                            info.m_undergroundMaterial.CopyPropertiesFromMaterial(info.m_material);
                        }
                    }
                    info.m_undergroundMaterial.SetVectorArray(instance.ID_TyreLocation, info.m_generatedInfo.m_tyres);
                    Graphics.DrawMesh(info.m_mesh, bodyMatrix, info.m_undergroundMaterial, instance.m_undergroundLayer, null, 0, materialBlock);
                }
                if (overground)
                {
                    info.m_material.SetVectorArray(instance.ID_TyreLocation, info.m_generatedInfo.m_tyres);
                    Graphics.DrawMesh(info.m_mesh, bodyMatrix, info.m_material, info.m_prefabDataLayer, null, 0, materialBlock);
                }
                // return false to skip the original method
                return false;
            }
            Matrix4x4 bodyMatrix2 = info.m_vehicleAI.CalculateBodyMatrix(flags, ref position, ref rotation, ref scale, ref swayPosition);
            Matrix4x4 topBodyMatrix2 = info.m_vehicleAI.CalculateBodyMatrix(flags, ref position, ref originalRotation, ref scale, ref Mod.TopSwayPosition);
            if (Singleton<ToolManager>.instance.m_properties.m_mode == ItemClass.Availability.AssetEditor)
            {
                Matrix4x4 value2 = info.m_vehicleAI.CalculateTyreMatrix(flags, ref position, ref rotation, ref scale, ref bodyMatrix2);
                VehicleManager instance2 = Singleton<VehicleManager>.instance;
                MaterialPropertyBlock materialBlock2 = instance2.m_materialBlock;
                materialBlock2.Clear();
                materialBlock2.SetMatrix(instance2.ID_TyreMatrix, value2);
                materialBlock2.SetVector(instance2.ID_TyrePosition, tyrePosition);
                materialBlock2.SetVector(instance2.ID_LightState, lightState);
                Mesh mesh = null;
                Material material = null;
                if (info.m_lodObject != null)
                {
                    MeshFilter component = info.m_lodObject.GetComponent<MeshFilter>();
                    if (component != null)
                    {
                        mesh = component.sharedMesh;
                    }
                    Renderer component2 = info.m_lodObject.GetComponent<Renderer>();
                    if (component2 != null)
                    {
                        material = component2.sharedMaterial;
                    }
                }
                if (mesh != null && material != null)
                {
                    materialBlock2.SetVectorArray(instance2.ID_TyreLocation, info.m_generatedInfo.m_tyres);
                    Graphics.DrawMesh(mesh, bodyMatrix2, material, info.m_prefabDataLayer, null, 0, materialBlock2);
                }
            }
            else if (Singleton<InfoManager>.instance.CurrentMode == InfoManager.InfoMode.None)
            {
                RenderGroup.MeshData effectMeshData2 = info.m_vehicleAI.GetEffectMeshData();
                EffectInfo.SpawnArea area2 = new EffectInfo.SpawnArea(bodyMatrix2, effectMeshData2, info.m_generatedInfo.m_tyres, info.m_lightPositions);
                if (info.m_effects != null)
                {
                    for (int k = 0; k < info.m_effects.Length; k++)
                    {
                        VehicleInfo.Effect effect2 = info.m_effects[k];
                        if (((effect2.m_vehicleFlagsRequired | effect2.m_vehicleFlagsForbidden) & flags) == effect2.m_vehicleFlagsRequired && effect2.m_parkedFlagsRequired == VehicleParked.Flags.None)
                        {
                            effect2.m_effect.RenderEffect(id, area2, velocity, acceleration, 1f, -1f, Singleton<SimulationManager>.instance.m_simulationTimeDelta, cameraInfo);
                        }
                    }
                }
            }
            bool flag3 = true;
            if (info.m_subMeshes != null)
            {
                for (int l = 0; l < info.m_subMeshes.Length; l++)
                {
                    VehicleInfo.MeshInfo meshInfo2 = info.m_subMeshes[l];
                    VehicleInfoBase subInfo2 = meshInfo2.m_subInfo;
                    if (((meshInfo2.m_vehicleFlagsRequired | meshInfo2.m_vehicleFlagsForbidden) & flags) == meshInfo2.m_vehicleFlagsRequired && (meshInfo2.m_variationMask & variationMask) == 0 && meshInfo2.m_parkedFlagsRequired == VehicleParked.Flags.None)
                    {
                        if (!(subInfo2 != null))
                        {
                            continue;
                        }
                        if (underground)
                        {

                            if (l == 1)
                            {
                                subInfo2.m_undergroundLodTransforms[subInfo2.m_undergroundLodCount] = topBodyMatrix2;
                            }
                            else
                            {
                                subInfo2.m_undergroundLodTransforms[subInfo2.m_undergroundLodCount] = bodyMatrix2;
                            }
                        
                            subInfo2.m_undergroundLodLightStates[subInfo2.m_undergroundLodCount] = lightState;
                            subInfo2.m_undergroundLodColors[subInfo2.m_undergroundLodCount] = color.linear;
                            subInfo2.m_undergroundLodMin = Vector3.Min(subInfo2.m_undergroundLodMin, position);
                            subInfo2.m_undergroundLodMax = Vector3.Max(subInfo2.m_undergroundLodMax, position);
                            if (++subInfo2.m_undergroundLodCount == subInfo2.m_undergroundLodTransforms.Length)
                            {
                                Vehicle.RenderUndergroundLod(cameraInfo, subInfo2);
                            }
                        }
                        if (overground)
                        {
                            if (l == 1)
                            {
                                subInfo2.m_lodTransforms[subInfo2.m_lodCount] = topBodyMatrix2;
                            }
                            else
                            {
                                subInfo2.m_lodTransforms[subInfo2.m_lodCount] = bodyMatrix2;
                            }
                            subInfo2.m_lodLightStates[subInfo2.m_lodCount] = lightState;
                            subInfo2.m_lodColors[subInfo2.m_lodCount] = color.linear;
                            subInfo2.m_lodMin = Vector3.Min(subInfo2.m_lodMin, position);
                            subInfo2.m_lodMax = Vector3.Max(subInfo2.m_lodMax, position);
                            if (++subInfo2.m_lodCount == subInfo2.m_lodTransforms.Length)
                            {
                                Vehicle.RenderLod(cameraInfo, subInfo2);
                            }
                        }
                    }
                    else if (subInfo2 == null)
                    {
                        flag3 = false;
                    }
                }
            }
            if (!flag3)
            {
                return false;
            }
            if (underground)
            {
                info.m_undergroundLodTransforms[info.m_undergroundLodCount] = bodyMatrix2;
                info.m_undergroundLodLightStates[info.m_undergroundLodCount] = lightState;
                info.m_undergroundLodColors[info.m_undergroundLodCount] = color.linear;
                info.m_undergroundLodMin = Vector3.Min(info.m_undergroundLodMin, position);
                info.m_undergroundLodMax = Vector3.Max(info.m_undergroundLodMax, position);
                if (++info.m_undergroundLodCount == info.m_undergroundLodTransforms.Length)
                {
                    Vehicle.RenderUndergroundLod(cameraInfo, info);
                }
            }
            if (overground)
            {
                info.m_lodTransforms[info.m_lodCount] = bodyMatrix2;
                info.m_lodLightStates[info.m_lodCount] = lightState;
                info.m_lodColors[info.m_lodCount] = color.linear;
                info.m_lodMin = Vector3.Min(info.m_lodMin, position);
                info.m_lodMax = Vector3.Max(info.m_lodMax, position);
                if (++info.m_lodCount == info.m_lodTransforms.Length)
                {
                    Vehicle.RenderLod(cameraInfo, info);
                }
            }
            // return false to skip the original method
            return false;
        }
    }
}
