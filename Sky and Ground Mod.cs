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
        public override string BesiegeVersion { get { return "v0.11"; } }
        public override string Author { get { return "覅是"; } }
        public override Version Version { get { return new Version(100, 100); } }
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
                
            }, "Changes wether or not you can see the red and blue spheres the next time you start the simulation");
            
        }

        //public int CurrentLevel = 2;
        IEnumerator groundTexture()
        {
            yield return new WaitForSeconds(0.01f);
            try { if (Input.GetKey(KeyCode.F5)) { GameObject.Find("FloorBig").renderer.material.mainTexture = null; GameObject.Find("FloorBig").renderer.material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.png").texture; } } catch (Exception c) { int i = 0; }
            try { if (Input.GetKey(KeyCode.F5)) { GameObject.Find("FloorBig").renderer.material.mainTexture = null; GameObject.Find("FloorBig").renderer.material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpg").texture; } } catch (Exception c) { int i = 0; }
            try { if (Input.GetKey(KeyCode.F5)) { GameObject.Find("FloorBig").renderer.material.mainTexture = null; GameObject.Find("FloorBig").renderer.material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.bmp").texture; } } catch (Exception c) { int i = 0; }
            try { if (Input.GetKey(KeyCode.F5)) { GameObject.Find("FloorBig").renderer.material.mainTexture = null; GameObject.Find("FloorBig").renderer.material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpeg").texture; } } catch (Exception c) { int i = 0; }
            try { if (Input.GetKey(KeyCode.F5)) { GameObject.Find("FloorBig").renderer.material.mainTexture = null; GameObject.Find("FloorBig").renderer.material.mainTexture = new WWW("File:///" + Application.dataPath + "/Mods/Blocks/Textures/GroundTexture.jpg").texture; } } catch (Exception c) { int i = 0; }

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
                        if (i < (int)clouds.Length/3)
                        {
                            clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(300, 400 - 40f), UnityEngine.Random.Range(-700, 700)), new Quaternion(0, 0, 0, 0));
                            clouds[i].particleSystem.startColor = new Color(1f, 1f, 1f, 1f);
                        }
                        else
                        {
                            clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(100f, 200), UnityEngine.Random.Range(-700, 700)), new Quaternion(0, 0, 0, 0));
                            clouds[i].particleSystem.startColor = new Color(0.76f + ((clouds[i].transform.position.y - 100) / 100) * 0.1f, 0.76f + ((clouds[i].transform.position.y - 100) / 100) * 0.1f, 0.8f, 1f);
                        }
                        clouds[i].SetActive(true);
                        clouds[i].particleSystem.startSize = 30;
                        clouds[i].transform.LookAt(new Vector3(UnityEngine.Random.Range(-500f, 700f), UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(-700, 700)));
                        clouds[i].particleSystem.startLifetime = 5;
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                        clouds[i].particleSystem.maxParticles = (int)clouds[i].transform.position.y;
                        

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
                        if (cloud.transform.position.x > 700) { cloud.transform.position = new Vector3(-700, cloud.transform.position.y, cloud.transform.position.z); }
                        if (cloud.transform.position.z > 700) { cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, -700); }

                    }
                }
            }
            catch { }
        }
        

    }




}
