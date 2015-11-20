using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using spaar.ModLoader;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace SkyAndCloud
{

    public class BesiegeModLoader : Mod
    {
        public override string Name { get { return "Sky_and_Ground_Mod"; } }
        public override string DisplayName { get { return "Sky and Ground Mod"; } }
        public override string BesiegeVersion { get { return "v0.2.0"; } }
        public override string Author { get { return "覅是"; } }
        public override Version Version { get { return new Version("0.79"); } }
        public override bool CanBeUnloaded { get { return true; } }

        public GameObject temp;



        public override void OnLoad()
        {


            GameObject temp = new GameObject();
            temp.AddComponent<SkyAndCloudMod>();
            GameObject.DontDestroyOnLoad(temp);

        }
        public override void OnUnload()
        {
            GameObject.Destroy(temp);
        }
    }

    public class SkyAndCloudMod : MonoBehaviour
    {

        private GameObject[] clouds = new GameObject[60];
        /*private GameObject floatingRock = new GameObject();
        private int floatingrocksCloneCount = 0;*/
        private GameObject cloudTemp;
        private GameObject godLightTemp;
        private GameObject rainTemp;
        private GameObject thunderCloudTemp;
        private int cloudAmount = 60;
        private int cloudAmountTemp = 0;
        private float cloudSizeScale = 1;
        private float lowerCloudsMinHeight = 130f;
        private float lowerCloudsMaxHeight = 200f;
        private float higherCloudsMinHeight = 300;
        private Color higherCloudsColor = new Color(1f, 1f, 1f, 1f);
        private Color lowerCloudsColor = new Color(0.92f, 0.9f, 0.8f, 1);
        private float[] cloudSpeed = new float[2];
        private float higherCloudsMaxHeight = 400 - 40;
        private bool resetCloudsNow = false;
        private bool CustomSpeed = false;
        private int tempLevel;
        private GameObject sunS = new GameObject();
        public Vector3 floorScale;
        private GameObject sun;
        public GameObject[] shadow;
        private bool isShadowoff = false;
        void OnLoad()
        {
            sun = new GameObject();
            sunS = new GameObject();
            DontDestroyOnLoad(sun.gameObject);
        }
        void Start()
        {
            //Application.LoadLevel (5);
            StartCoroutine(groundTexture());

            Commands.RegisterHelpMessage("SimpleIOBlocks commands:\n	IOSpheres [bool]\n	IOPulse [bool]\n	IOTickGUI [bool]");
            Commands.RegisterCommand("ResetCloudsAmount", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    int cccloudcloudAmount = int.Parse(args[0]);
                    if (cccloudcloudAmount < 0 ^ cccloudcloudAmount > 3000) { return "Your cloud amount is not available. "; }
                    else { cloudAmount = cccloudcloudAmount; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to cloud amount";
                }
                return "There will be " + cloudAmount.ToString() + " clouds";


            }, "Reset the amount of clouds. No bigger than 3000 and no less than 2.");//Amount

            Commands.RegisterCommand("ResetCloudsSizeScale", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float cccloudcloudSizeScale = float.Parse(args[0]);
                    if (cccloudcloudSizeScale <= 0) { return "Your cloud size scale is not available. "; }
                    else { cloudSizeScale = cccloudcloudSizeScale; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to cloud size scale";
                }

                return "The clouds' size scale will be " + cloudSizeScale.ToString();

            }, "Reset Clouds' Size Scale");//SizeScale

            Commands.RegisterCommand("ResetLowerCloudsMinHeight", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float llllowerCloudsMinHeight = float.Parse(args[0]);
                    if (llllowerCloudsMinHeight >= lowerCloudsMaxHeight) { return "Your lower cloud minimum height is not available. "; }
                    else { lowerCloudsMinHeight = llllowerCloudsMinHeight; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to lower cloud minimum height";
                }

                return "The lower clouds' minimum height will be " + lowerCloudsMinHeight.ToString();

            }, "Reset Lower Clouds' Min Height");//ResetLowerCloudsMinHeight

            Commands.RegisterCommand("ResetLowerCloudsMaxHeight", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float llllowerCloudsMaxHeight = float.Parse(args[0]);
                    if (llllowerCloudsMaxHeight <= lowerCloudsMinHeight) { return "Your lower cloud maximum height is not available. "; }
                    else { lowerCloudsMaxHeight = llllowerCloudsMaxHeight; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to lower cloud maximum height";
                }

                return "The lower clouds' maximum height will be " + lowerCloudsMaxHeight.ToString();

            }, "Reset Lower Clouds' Max Height");//ResetLowerCloudsMaxHeight

            Commands.RegisterCommand("ResetHigherCloudsMinHeight", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float hhhhigherCloudsMinHeight = float.Parse(args[0]);
                    if (hhhhigherCloudsMinHeight >= higherCloudsMaxHeight) { return "Your higher cloud minimum height is not available. "; }
                    else { higherCloudsMinHeight = hhhhigherCloudsMinHeight; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to higher cloud minimum height";
                }

                return "The higher clouds' minimum height will be " + higherCloudsMinHeight.ToString();

            }, "Reset Higher Clouds' Min Height");//ResetHigherCloudsMinHeight

            Commands.RegisterCommand("ResetHigherCloudsMaxHeight", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float hhhigherCloudsMaxHeight = float.Parse(args[0]);
                    if (hhhigherCloudsMaxHeight <= higherCloudsMinHeight) { return "Your higher cloud maximum height is not available. "; }
                    else { higherCloudsMaxHeight = hhhigherCloudsMaxHeight; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to higher cloud maximum height";
                }

                return "The higher clouds' maximum height will be " + higherCloudsMaxHeight.ToString();

            }, "Reset Higher Clouds' Max Height.");//ResetHigherCloudsMaxHeight

            Commands.RegisterCommand("ResetHigherCloudsColorRGBA", (args, notUses) =>
            {
                if (args.Length < 3)
                {
                    return "ERROR!You don't have all four color elements! (Red, Green, Blue, Alpha) \n Please do it like this\n  ResetHigherCloudsColorRGBA 155 0 255 99";
                }
                try
                {
                    higherCloudsColor = new Color(float.Parse(args[0]) / 255f, float.Parse(args[1]) / 255f, float.Parse(args[2]) / 255f, float.Parse(args[3]) / 100f);
                }
                catch
                {
                    return "ERROR! Please do it like this\n  ResetHigherCloudsColorRGBA 155 0 255 99";
                }

                return "The higher cloud color will be " + higherCloudsColor.ToString();

            }, "Reset the color of higher clouds by R G B A.");//ResetHigherCloudsColor

            Commands.RegisterCommand("ResetLowerCloudsColorRGBA", (args, notUses) =>
            {
                if (args.Length < 3)
                {
                    return "ERROR!You don't have all four color elements! (Red, Green, Blue, Alpha) \n Please do it like this\n  ResetLowerCloudsColorRGBA 155 0 255 99";
                }
                try
                {
                    lowerCloudsColor = new Color(float.Parse(args[0]) / 255f, float.Parse(args[1]) / 255f, float.Parse(args[2]) / 255f, float.Parse(args[3]) / 100f);
                }
                catch
                {
                    return "ERROR! Please do it like this\n  ResetLowCloudsColorRGBA 155 0 255 99";
                }

                return "The lower cloud color will be " + lowerCloudsColor.ToString();

            }, "Reset the color of lower clouds by R G B A.");//ResetLowerCloudsColor

             Commands.RegisterCommand("ResetSkyColorRGBA", (args, notUses) =>
            {
                if (args.Length < 3)
                {
                    return "ERROR!You don't have all four color elements! (Red, Green, Blue, Alpha) \n Please do it like this\n ResetSkyColorRGBA 155 0 255 99";
                }
                try
                {
                    GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = new Color(float.Parse(args[0]) / 255f, float.Parse(args[1]) / 255f, float.Parse(args[2]) / 255f, float.Parse(args[3]) / 100f);
                }
                catch
                {
                    return "ERROR! Please do it like this\n ResetSkyColorRGBA 155 0 255 99";
                }

                return "The sky color will be " + lowerCloudsColor.ToString();

            }, "Reset the color of sky by R G B A. Default is 144 166 180 100");//ResetSkyColor

            Commands.RegisterCommand("Re-ProduceAllClouds", (args, notUses) =>
            {

                resetCloudsNow = true;
                return "The clouds will be re-produce";

            }, "Produce your clouds again");//Reproduce All Clouds

            Commands.RegisterCommand("CleanFog", (args, notUses) =>
            {

                try { GameObject.Find("Fog Volume").transform.position = new Vector3(0, Mathf.Infinity, 0); return "The Fog will be moved away"; } catch { return "The Fog does not exist!"; }

            }, "Put the fog away to make your view cleaner");//Clean Fog

            Commands.RegisterCommand("ResetFog", (args, notUses) =>
            {

                try { GameObject.Find("Fog Volume").transform.position = new Vector3(0, GameObject.Find("Main Camera").transform.position.y - 50, 0); return "The Fog will be reset under your camera"; } catch { return "The Fog does not exist!"; }

            }, "Put the fog back");//Reset Fog

            Commands.RegisterCommand("ResetCloudSpeed", (args, notUses) =>
            {

                if (args.Length < 2)
                {
                    return "ERROR!You need to have two speed value!";
                }
                try
                {
                    cloudSpeed[0] = float.Parse(args[0]);
                    cloudSpeed[1] = float.Parse(args[1]);
                }
                catch
                {
                    return "ERROR!";
                }
                CustomSpeed = true;
                return "The speed will be: X: " + cloudSpeed[0] + "  Z: " + cloudSpeed[1];

            }, "Change the moving speed of yur clouds by x and z");//Cloud speed

            Commands.RegisterCommand("ResetFloorSizeScale", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    GameObject.Find("FloorBig").transform.localScale = new Vector3(float.Parse(args[0]), GameObject.Find("FloorBig").transform.localScale.y, float.Parse(args[1]));
                }
                catch
                {
                    return "Could not parse " + args[0] + "and" + args[1] + "to floor size scale";
                }

                return "The floor's size scale will be " + GameObject.Find("FloorBig").transform.localScale.ToString();

            }, "Reset the size of the floor as big as you want.(default is 900 900)");//FloorSizeScale

            Commands.RegisterCommand("ResetCameraDrawingRange", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float cr = float.Parse(args[0]);
                    if (cr <= 1 ) { return "Your Range is not available. "; }
                    else { GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane = cr; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to camara drawing range";
                }

                return "The camara drawing range will be " + GameObject.Find("Main Camera").GetComponent<Camera>().farClipPlane.ToString();

            }, "Reset the camera drawing range to the value you want (no less than 1; default is 1500)");//ResetCameraDrawingRange

            Commands.RegisterCommand("TurnOn/OffCloudShadows", (args, notUses) =>
            {

                try {
                    if (!isShadowoff)
                    {
                        foreach (GameObject shadowMaker in shadow) { shadowMaker.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; }
                        isShadowoff = true;
                        return "The shadow has been turned off";
                        
                    }

                    else if (isShadowoff)
                    {
                        foreach (GameObject shadowMaker in shadow) { shadowMaker.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly; }
                        isShadowoff = false;
                        return "The shadow has been turned on";
                    }
                    else { return "Nothing Can be turn off/on"; }
                } catch { return "The shadows does not exist!"; }

            }, "Turn on/off your clouds' shadows");//Shadow

           /* Commands.RegisterCommand("AddOneFloatingRock", (args, notUses) =>
            {
            if (args.Length < 3)
            {
                return "ERROR!You don't have all four position and rotation values! (X, Y, Z, Rotation) \n Please do it like this\n  AddOneFloatingRock -120 110 56 92";
            }
                if (Application.loadedLevel == 23 ^ floatingRock != null)
                {
                    floatingRock = GameObject.Find("FloatingRocks");
                    GameObject.DontDestroyOnLoad(floatingRock);
                    DontDestroyOnLoad(floatingRock);
                    floatingRock.transform.parent = null;
                    floatingRock.SetActive(false);

                    try
                    {
                        Quaternion qtnon = new Quaternion();
                        qtnon.eulerAngles = new Vector3(0, float.Parse(args[3]), 0);
                        GameObject floatingrocksClone = new GameObject();
                        floatingrocksCloneCount += 1;
                        floatingrocksClone.name = ("floatingrocksClone" + floatingrocksCloneCount);
                        floatingrocksClone = (GameObject)GameObject.Instantiate(floatingRock, new Vector3(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2])), qtnon);
                        floatingrocksClone.SetActive(true);
                    }
                    catch 
                    {
                        if (GameObject.Find("FloatingRocks"))
                        {
                            return "ERROR! Please do it like this\n  AddOneFloatingRock -120 110 56 93";
                        }
                        else { return "You need to go Level 18 to get FloatingRocks!"; }
                    }
                }
                else
                {
                    return "The" + floatingrocksCloneCount + "'s stone will be " + GameObject.Find("floatingrocksClone" + floatingrocksCloneCount).transform.position.ToString();
                }
                return "a";

            }, "Reset the color of lower clouds by X Y Z Rotation.");//AddOneFloatingRock

            Commands.RegisterCommand("DeleteAllFloatingStones", (args, notUses) =>
            {

                try
                {
                    if (floatingRocks.Length < 1) { return "No Rocks!"; }
                    else
                    {
                        for (int i = floatingRocks.Length; i >= 0; i--)
                        {
                            Destroy(floatingRocks[i]);
                        }

                        return "Done!";
                    }
                }
                catch { return "The stones does not existed!"; }

            }, "Delete All Floating Stones");//DeleteAllFloatingStones

            Commands.RegisterCommand("DeleteLatestFloatingStone", (args, notUses) =>
            {

                try
                {
                    if (floatingRocks.Length < 1) { return "No Rocks!"; }
                    else
                    {
                        Destroy(floatingRocks[floatingRocks.Length]);
                        

                        return "Done!";
                    }
                }
                catch { return "The stones does not existed!"; }

            }, "Delete Latest Floating Stone");//DeleteLatestFloatingStones

            Commands.RegisterCommand("ResetLatestStoneSizeScale", (args, notUses) =>
            {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    float stoneSizeScale = float.Parse(args[0]);
                    if (stoneSizeScale <= 0) { return "Your stone size scale is not available. "; }
                    else { floatingRocks[floatingRocks.Length].transform.localScale = new Vector3(stoneSizeScale, stoneSizeScale, stoneSizeScale); }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to stone size scale";
                }

                return "The clouds' size scale will be " + floatingRocks[floatingRocks.Length].transform.localScale.x.ToString();

            }, "Reset the latest Stone's Size Scale");//StoneSizeScale*/

            Commands.RegisterCommand("NoWorldBoundaries", (args, notUses) =>
            {

                try { GameObject.Find("WORLD BOUNDARIES").transform.localScale = new Vector3(0, 0, 0); return "The World Boundaries will be moved away"; } catch { return "The World Boundaries does not exist!"; }

            }, "Move the World Boundaries away");//Clean World Boundaries

            Commands.RegisterCommand("ResetWorldBoundaries", (args, notUses) =>
            {

                try { GameObject.Find("WORLD BOUNDARIES").transform.position = new Vector3(1,1,1); return "The World Boundaries will be reset."; } catch { return "The World Boundaries does not exist!"; }

            }, "Put the World Boundaries back");//Reset World Boundaries

        /*    Commands.RegisterCommand("TryFloatingStones", (args, notUses) =>
            {

                try { AsyncOperation poop = Application.LoadLevelAsync(22); return poop + "Level will be load."; } catch { return "The Level cannot be load!"; }
                
            }, "Try load floating stones");//Try load Floating Stones*/


        }

        //public int CurrentLevel = 2;
        IEnumerator groundTexture()
        {
            yield return new WaitForSeconds(0.01f);
            try
            {

                if (Input.GetKey(KeyCode.F5))
                {
                    WWW png = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.png");
                    WWW jpg = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpg");
                    GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture = null;
                    if (png.size > 5)
                    {
                        try
                        {
                            GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.png").texture;
                        }
                        catch { }
                        try
                        {
                            GameObject.Find("terrainObject").GetComponent<Renderer>().material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.png").texture;
                        }
                        catch { }
                    }
                    else if (jpg.size > 5)
                    {
                        try
                        {
                            GameObject.Find("FloorBig").GetComponent<Renderer>().material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpg").texture;
                        }
                        catch { }
                        try
                        {
                            GameObject.Find("terrainObject").GetComponent<Renderer>().material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpg").texture;
                        }
                        catch { }
                    }
                    else { Debug.Log("There is no such a texture file named \"GroundTexture.png\" or \"GroundTexture.jpg\" \n under \\Besiege_Data\\Mods\\Blocks\\Textures\\! "); }
                }
            }
            catch { }
            StartCoroutine(groundTexture());
        }


        void Update()
        {
            //Debug.Log(Application.loadedLevel);
            try
            {
                /*if (GameObject.Find("Directional light").GetComponent<Light>().flare == null)
                {
                    LensFlare sunf = new LensFlare();
                    sunf.color = Color.white;
                    sunf.brightness = 10;
                    sunf.fadeSpeed = 0;
                                    }*/
                if (!GameObject.Find("new sun")) {
                    /*sun = new GameObject();
                    
                    
                    sun.name = "new sun";
                    sun.AddComponent<Light>();
                    sun.AddComponent<LensFlare>();
                    sun.GetComponent<LensFlare>().color = Color.yellow;
                    sun.GetComponent<LensFlare>().brightness = 10;
                    sun.GetComponent<LensFlare>().fadeSpeed = 0;
                    sun.GetComponent<Light>().type = LightType.Point;
                    sun.GetComponent<Light>().range = 3000;
                   // sun.GetComponent<Light>().color = Color.red;
                    sun.GetComponent<Light>().intensity = 1.1f;
                    sun.GetComponent<Light>().shadows = LightShadows.Soft;
                    sun.transform.position = Vector3.up * 100;*/
                }
                GameObject.Find("Directional light").transform.eulerAngles = new Vector3(GameObject.Find("Directional light").transform.eulerAngles.x, 120, GameObject.Find("Directional light").transform.eulerAngles.z);
                GameObject.Find("Directional light").GetComponent<Light>().shadows = LightShadows.Soft;
                /*if (!GameObject.Find("Sun Sphere"))
                {
                    GameObject sunS = new GameObject();
                    sunS = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sunS.GetComponent<Rigidbody>().isKinematic = false;
                    Destroy(sunS.GetComponent<Collider>());
                    sunS.GetComponent<Renderer>().receiveShadows = false;
                    sunS.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    sunS.name = "Sun Sphere";
                    sunS.transform.localScale = new Vector3(200, 200, 200);
                }

                try
                {
                    sunS.transform.position = GameObject.Find("Directional light").transform.forward * -700 + GameObject.Find("3D Hud Cam").transform.position;
                    Debug.Log("TRIED!");
                    sun.transform.position = GameObject.Find("Directional light").transform.forward * -600 + GameObject.Find("3D Hud Cam").transform.position;
                }
                catch { }*/
                
                /* WWW flare = new WWW("File:///" + Application.dataPath + "/Mods/50mm Zoom.flare");
                 UnityEngine.Flare flareO = flare.assetBundle.LoadAsset("50mm Zoom");
                 Flare anoFlare = UnityEngine.Object.Instantiate(flareO, GameObject.Find("Directional light").transform.position, GameObject.Find("Directional light").transform.rotation);
                 GameObject.Find("Directional light").AddComponent<Light>().flare = anoFlare;*/



            }
            catch {}
            
                if (tempLevel != Application.loadedLevel) {
                try
                {
                   // GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = Color.cyan;
                    tempLevel = Application.loadedLevel;
                }
                catch { }
                }
            if (cloudTemp == null) { cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud")); cloudTemp.SetActive(false); }
            DontDestroyOnLoad(cloudTemp);
            if (cloudAmountTemp != cloudAmount) { resetCloudsNow = true; clouds[1] = null; cloudAmountTemp = cloudAmount; try { for (int k = cloudAmount; k < clouds.Length; k++) { Destroy(clouds[k].gameObject); Destroy(shadow[k].gameObject); } } catch { } }
            try
            {
                floorScale = GameObject.Find("FloorBig").transform.localScale;
                if (clouds[1] == null && cloudAmount > 1)
                {
                    clouds = new GameObject[cloudAmount];
                    shadow = new GameObject[cloudAmount];
                    for (int i = 0; i <= clouds.Length; i++)
                    {
                        
                        GameObject.DontDestroyOnLoad(clouds[i]);
                        if (i < (int)clouds.Length / 3)
                        {
                            clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(higherCloudsMinHeight, higherCloudsMaxHeight), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)), new Quaternion(0, 0, 0, 0));
                            clouds[i].GetComponent<ParticleSystem>().startColor = higherCloudsColor;
                            clouds[i].layer = 12;
                        }
                        else
                        {
                            clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(lowerCloudsMinHeight, lowerCloudsMaxHeight), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)), new Quaternion(0, 0, 0, 0));
                            clouds[i].GetComponent<ParticleSystem>().startColor = lowerCloudsColor;
                            clouds[i].layer = 12;
                        }
                        clouds[i].SetActive(true);
                        clouds[i].GetComponent<ParticleSystem>().startSize = 30;
                        clouds[i].GetComponent<ParticleSystem>().startLifetime = 5;
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                        clouds[i].GetComponent<ParticleSystem>().maxParticles = (int)clouds[i].transform.position.y;
                        shadow[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        shadow[i].GetComponent<Collider>().transform.parent = shadow[i].transform;
                        shadow[i].layer = clouds[i].layer;
                        shadow[i].transform.position = clouds[i].transform.position;
                        shadow[i].transform.parent = clouds[i].transform;
                        shadow[i].transform.localPosition = new Vector3(0.5f, 0, 0);
                        shadow[i].transform.localEulerAngles = new Vector3(18,10,353);
                        shadow[i].transform.localScale = new Vector3(4,2.5f,2.5f);
                        shadow[i].GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                        shadow[i].GetComponent<Renderer>().receiveShadows = true;
                        Destroy(shadow[i].GetComponent<Renderer>().material.mainTexture);
                        clouds[i].transform.LookAt(new Vector3(UnityEngine.Random.Range(-floorScale.x / 2 - 200, floorScale.x / 2 + 200), UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(-floorScale.z / 2 - 200, floorScale.z / 2 + 200)));
                        try
                        {
                          //  clouds[i].GetComponent<ParticleRenderer>().receiveShadows = true;
                            clouds[i].GetComponent<ParticleSystemRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                        }
                        catch { Debug.Log("Shadow failed!"); }




                    }
                }
                else
                {
                    foreach (GameObject cloud in clouds)
                    {
                        float randomMove = UnityEngine.Random.Range(0.01f, 0.02f);

                        if (Application.loadedLevel == 2) { cloud.transform.position = new Vector3(-9999, -9999, -9999); }
                        if (CustomSpeed) { cloud.transform.position += new Vector3(cloudSpeed[0], randomMove - 0.015f, cloudSpeed[1]); }
                        else
                        {
                            cloud.transform.position += new Vector3(randomMove, randomMove - 0.015f, randomMove);
                        }
                        cloud.transform.localScale *= 1 + randomMove - 0.015f;
                        cloud.GetComponent<ParticleSystem>().startLifetime = 0.01f;
                        cloud.GetComponent<ParticleSystem>().startSize = cloudSizeScale * 30;
                        cloud.transform.localScale = new Vector3(15 * cloudSizeScale, 15 * cloudSizeScale, 15 * cloudSizeScale);
                        cloud.GetComponent<ParticleSystem>().startLifetime = 5;

                        if (cloud.transform.position.x > floorScale.x / 2 + 200) { cloud.transform.position = new Vector3(-floorScale.x / 2 - 195, cloud.transform.position.y, cloud.transform.position.z); }
                        if (cloud.transform.position.z > floorScale.z / 2 + 200) { cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, -floorScale.z / 2 - 195); }
                        if (cloud.transform.position.x < -floorScale.x / 2 - 200) { cloud.transform.position = new Vector3(floorScale.x / 2 + 195, cloud.transform.position.y, cloud.transform.position.z); }
                        if (cloud.transform.position.z < -floorScale.z / 2 - 200) { cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, floorScale.z / 2 + 195); }

                    }
                }
                if (resetCloudsNow)
                {
                    resetCloudsNow = false;
                    foreach (GameObject cloud in Resources.FindObjectsOfTypeAll(typeof(GameObject))) { if (cloud != cloudTemp && cloud.name.Equals("CLoud(Clone)(Clone)")) { Destroy(cloud); } }
                    clouds = new GameObject[cloudAmount];
                }
                foreach (GameObject oneShadow in shadow) { if (isShadowoff) { oneShadow.transform.localScale = new Vector3(0, 0, 0); } else { oneShadow.transform.localScale = new Vector3(4, 2.5f, 2.5f); } }
            }
            catch { }

            try { if (godLightTemp != null) { godLightTemp = GameObject.Find("GodRays"); godLightTemp.SetActive(false); } } catch { }
            try { if (rainTemp != null) { rainTemp = GameObject.Find("Rain Particles"); rainTemp.SetActive(false); } } catch { }
            try { if (thunderCloudTemp != null) { thunderCloudTemp = GameObject.Find("THUNDER CLOUD"); thunderCloudTemp.SetActive(false); } } catch { }
        }
     /*   void OnParticleCollision(GameObject other)
        {
            Rigidbody body = other.GetComponent<Rigidbody>();

            Vector3 direction = transform.position - other.transform.position;
            direction = direction.normalized;
           
            this.GetComponent<ParticleAddForce>().particleForce(body.GetRelativePointVelocity(body.transform.position));
            Debug.Log("push!");


        }*/





    }
}
