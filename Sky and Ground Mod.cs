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
        public override string BesiegeVersion { get { return "v0.2"; } }
        public override string Author { get { return "覅是"; } }
        public override Version Version { get { return new Version("0.73"); } }
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
        private GameObject cloudTemp;
        private int cloudAmount = 60;
        private int cloudAmountTemp = 0;
        private float cloudSizeScale = 1;
        private float lowerCloudsMinHeight = 130f;
        private float lowerCloudsMaxHeight = 200f;
        private float higherCloudsMinHeight = 300;
        private Color higherCloudsColor = new Color(1f, 1f, 1f, 1f);
        private Color lowerCloudsColor = new Color (0.92f, 0.9f,0.8f,1);
        private float higherCloudsMaxHeight = 400-40;
        private bool resetCloudsNow = false;
        void Start()
        {
            //Application.LoadLevel (5);
            StartCoroutine(groundTexture());



            Commands.RegisterHelpMessage("SimpleIOBlocks commands:\n	IOSpheres [bool]\n	IOPulse [bool]\n	IOTickGUI [bool]");
             Commands.RegisterCommand("ResetCloudsAmount", (args, notUses) => {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    int cccloudcloudAmount = int.Parse(args[0]);
                    if (cccloudcloudAmount <= 1 ^ cccloudcloudAmount > 3000) { return "Your cloud amount is not available. ";  }
                    else { cloudAmount = cccloudcloudAmount; }
                }
                catch
                {
                    return "Could not parse " + args[0] + "to cloud amount";
                }
                return "There will be " + cloudAmount.ToString() + " clouds";
                
                
            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//Amount
            
            Commands.RegisterCommand("ResetCloudsSizeScale", (args, notUses) => {
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

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//SizeScale

            Commands.RegisterCommand("ResetLowerCloudsMinHeight", (args, notUses) => {
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

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//ResetLowerCloudsMinHeight

            Commands.RegisterCommand("ResetLowerCloudsMaxHeight", (args, notUses) => {
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

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//ResetLowerCloudsMaxHeight

            Commands.RegisterCommand("ResetHigherCloudsMinHeight", (args, notUses) => {
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

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//ResetHigherCloudsMinHeight

             Commands.RegisterCommand("ResetHigherCloudsMaxHeight", (args, notUses) => {
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

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//ResetHigherCloudsMaxHeight

            Commands.RegisterCommand("ResetHigherCloudsColorRGBA", (args, notUses) => {
                if (args.Length < 3)
                {
                    return "ERROR!You don't have all four color elements! (Red, Green, Blue, Alpha) \n Please do it like this\n  ResetHigherCloudsColorRGBA 155 0 255 99";
                }
                try
                {
                    higherCloudsColor = new Color(float.Parse(args[0])/255f, float.Parse(args[1])/255f, float.Parse(args[2])/255f, float.Parse(args[3])/100f);
                }
                catch
                {
                    return "ERROR! Please do it like this\n  ResetHigherCloudsColorRGBA 155 0 255 99";
                }

                return "The higher cloud color will be " + higherCloudsColor.ToString();

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//ResetHigherCloudsColor

            Commands.RegisterCommand("ResetLowerCloudsColorRGBA", (args, notUses) => {
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

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//ResetLowerCloudsColor

             Commands.RegisterCommand("Re-ProduceAllClouds", (args, notUses) => {

                resetCloudsNow = true;
                return "The clouds will be re-produce";

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//Reproduce All Clouds

            Commands.RegisterCommand("ResetFloorSizeScale", (args, notUses) => {
                if (args.Length < 1)
                {
                    return "ERROR!";
                }
                try
                {
                    GameObject.Find("FloorBig").transform.localScale = new Vector3(float.Parse(args[0]), GameObject.Find("FloorBig").transform.localScale.y,float.Parse(args[1]));
                }
                catch
                {
                    return "Could not parse " + args[0] + "and" + args[1] + "to floor size scale";
                }

                return "The floor's size scale will be " + GameObject.Find("FloorBig").transform.localScale.ToString();

            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");//FloorSizeScale
        }

        //public int CurrentLevel = 2;
        IEnumerator groundTexture()
        {
            yield return new WaitForSeconds(0.01f);
            try {
                
                if (Input.GetKey(KeyCode.F5)) {
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
                } } catch  {}
            StartCoroutine(groundTexture());
        }
        
            void Update()
        {
            if (cloudTemp == null) { cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud")); cloudTemp.SetActive(false); }
            DontDestroyOnLoad(cloudTemp);
            if (cloudAmountTemp != cloudAmount) { clouds[1] = null; cloudAmountTemp = cloudAmount; try { for (int k = cloudAmount; k < clouds.Length; k++) { Destroy(clouds[k]); } } catch { } }
            try
            {
                if (clouds[1] == null)
                {
                    clouds = new GameObject[cloudAmount];
                    for (int i = 0; i <= clouds.Length; i++)
                    {
                        GameObject.DontDestroyOnLoad(clouds[i]);
                        if (i < (int)clouds.Length / 3)
                        {
                            clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(higherCloudsMinHeight, higherCloudsMaxHeight), UnityEngine.Random.Range(-700, 700)), new Quaternion(0, 0, 0, 0));
                            clouds[i].GetComponent<ParticleSystem>().startColor = higherCloudsColor;
                        }
                        else
                        {
                            clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(lowerCloudsMinHeight, lowerCloudsMaxHeight), UnityEngine.Random.Range(-700, 700)), new Quaternion(0, 0, 0, 0));
                            clouds[i].GetComponent<ParticleSystem>().startColor = new Color(lowerCloudsColor.r, lowerCloudsColor.g/* + ((clouds[i].transform.position.y - lowerCloudsMinHeight) / lowerCloudsMaxHeight - lowerCloudsMinHeight) * 0.1f*/, lowerCloudsColor.b, lowerCloudsColor.a);
                        }
                        clouds[i].SetActive(true);
                        clouds[i].GetComponent<ParticleSystem>().startSize = 30;
                        clouds[i].transform.LookAt(new Vector3(UnityEngine.Random.Range(-500f, 700f), UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(-700, 700)));
                        clouds[i].GetComponent<ParticleSystem>().startLifetime = 5;
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                        clouds[i].GetComponent<ParticleSystem>().maxParticles = (int)clouds[i].transform.position.y;


                    }
                }
                else
                {
                    foreach (GameObject cloud in clouds)
                    {
                        float randomMove = UnityEngine.Random.Range(0.01f, 0.02f);

                        if (Application.loadedLevel == 2) { cloud.transform.position = new Vector3(-9999, -9999, -9999); }
                        cloud.transform.position += new Vector3(randomMove, randomMove - 0.015f, randomMove);
                        cloud.transform.localScale *= 1 + randomMove - 0.015f;
                        cloud.GetComponent<ParticleSystem>().startLifetime = 0.01f;
                        cloud.GetComponent<ParticleSystem>().startSize = cloudSizeScale * 30;
                        cloud.transform.localScale = new Vector3(15 * cloudSizeScale, 15 * cloudSizeScale, 15 * cloudSizeScale);
                        cloud.GetComponent<ParticleSystem>().startLifetime = 5;
                        if (cloud.transform.position.x > 700) { cloud.transform.position = new Vector3(-700, cloud.transform.position.y, cloud.transform.position.z); }
                        if (cloud.transform.position.z > 700) { cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, -700); }

                    }
                }
                if (resetCloudsNow)
                {
                    resetCloudsNow = false;
                    foreach (GameObject cloud in Resources.FindObjectsOfTypeAll(typeof(GameObject))) { if (cloud != cloudTemp && cloud.name.Equals("CLoud(Clone)(Clone)")) { Destroy(cloud); } }
                        clouds = new GameObject[cloudAmount];
                }
            }
            catch { } 
        }
        

    }




}
