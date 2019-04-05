MixCast SDK for Unity - v2.2.0
(c) Blueprint Reality Inc., 2019. All rights reserved
https://mixcast.me

Basic Installation:
1) Instantiate the "MixCast SDK" prefab found in Assets/MixCast/Prefabs in your first scene
2) Ensure MixCast is running (in the system tray) or open it from the Start Menu
3) Run your application and a MixCast output window should launch automatically!

Comprehensive instructions can be found here: https://mixcast.me/docs/develop/unity

Note: When upgrading from a previous version of MixCast, please delete the old MixCast folder(s) before importing the new package.
	  If MixCast or any other folder re-imports right after deleting, please close Unity and any code editors (Visual Studio, MonoDevelop) or other programs that may be accessing script files and try again.

Project Requirements:
- Unity 5.4.0 or above
- Windows Standalone x64
- OpenVR or Oculus

The separately packaged MixCast client must be installed, configured, and run to enable MixCast output in your application at runtime.


Extras:
MixCast also comes with some extra prefabs and scripts to aid with mixed reality development and production.

Slate UI prefab - Provides a film slate style display that can be called up via keypress to aid in the capture of multiple takes in a row. Inspect the attached script for more details. Drop into any scene.

Player Blob Shadow prefabs - Causes a simple blob shadow to appear in MixCast output at the point on the ground where the player's head is over. Attach to the Head or Eye transform.

SetRenderingForMixCast script - Controls whether the specified Renderer components are visible during regular rendering or MixCast rendering rather than both. By default grabs all Renderers under it in the hierarchy.


Known Issues:
- MixCast and the Editor must be running at the same permission level to communicate (either both Run as Admin or neither)


Additional Info:

MixCast Changelist - https://mixcast.me/route.php?dest=sdk_changelist_unity
MixCast User and Developer Documentation - https://mixcast.me/docs/
MixCast Support - https://support.blueprintreality.com/