// By default the cable car template uses the ship shader after import
// this script makes the asset use the default vehicle shader
var shader = Shader.Find("Custom/Vehicles/Vehicle/Default");
var asset = ToolsModifierControl.toolController.m_editPrefabInfo as VehicleInfo;
if(asset.m_material != null) asset.m_material.shader = shader;
if(asset.m_lodMaterial != null) asset.m_lodMaterial.shader = shader;

