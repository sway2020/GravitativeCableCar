// If the prop version used the default name "New Asset" for the main part,
// and "New Asset1" for the top/wheel part, no need to change anything here.
// Else, change the names. Remember to keep the _Data thing at the end. 

var main_prop_version = PrefabCollection<PropInfo>.FindLoaded("New Asset_Data");
var top_prop_version = PrefabCollection<PropInfo>.FindLoaded("New Asset1_Data");

// You probably need to change this part to set the tire parameters if you want spinning wheels
// I don't have them so this part isn't useful to me.

Vector4[] tyres = new Vector4[] {
new Vector4(-0.739f, 0.328f, 1.421f, 0.328f)
};
Vector4[] tyres1 = new Vector4[] {
new Vector4(-0.739f, 0.328f, 1.421f, 0.328f)
};

// This part copies vertex colors from the prop version to the vehicle version only for the
// main part and the top/wheel part(main mehs and first submesh). 
// I think Ronyx69's rotor shader script will still work for other submeshes if you want to have transparent windows
// but I never tried to use rotor shader so I don't know how it works.
// No need to change if you only have one submesh(for the top/wheel part).

var main_vehicle_version = ToolsModifierControl.toolController.m_editPrefabInfo as VehicleInfo;
Color[] colors = new Color[main_prop_version.m_mesh.vertices.Length];
for (int i = 0; i < main_prop_version .m_mesh.vertices.Length; i++) colors[i] = main_prop_version .m_mesh.colors[i];
main_vehicle_version.m_mesh.colors = colors;
main_vehicle_version.m_generatedInfo.m_tyres = tyres;

var sm = (ToolsModifierControl.toolController.m_editPrefabInfo as VehicleInfo).m_subMeshes[1].m_subInfo;
Color[] colors1 = new Color[top_prop_version.m_mesh.vertices.Length];
for (int i = 0; i < top_prop_version .m_mesh.vertices.Length; i++) colors1[i] = top_prop_version .m_mesh.colors[i];
sm.m_mesh.colors = colors1;
sm.m_generatedInfo.m_tyres = tyres1;
