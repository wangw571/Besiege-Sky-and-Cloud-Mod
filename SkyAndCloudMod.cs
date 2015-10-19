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
        public override string Name { get { return "SkyAndCloudMod"; } }
        public override string DisplayName { get { return "Sky and Cloud Mod"; } }
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
        void Start()
        {
            //Application.LoadLevel (5);
        }

        //public int CurrentLevel = 2;

        
            void Update()
        {
            if (cloudTemp == null) { cloudTemp = (GameObject)UnityEngine.Object.Instantiate(GameObject.Find("CLoud")); cloudTemp.SetActive(false); }
            DontDestroyOnLoad(cloudTemp);
            if (clouds[1] == null)
            {
                for (int i = 0; i <= clouds.Length; i++) { try { GameObject.DontDestroyOnLoad(clouds[i]);
                        clouds[i] = (GameObject)UnityEngine.Object.Instantiate(cloudTemp, new Vector3(UnityEngine.Random.Range(-700f, 700f), 400-70f, UnityEngine.Random.Range(-700, 700)), new Quaternion(0, 0, 0, 0));
                        clouds[i].SetActive(true);
                        clouds[i].particleSystem.startSize = 30;
                        clouds[i].transform.LookAt(new Vector3(UnityEngine.Random.Range(-500f, 700f), UnityEngine.Random.Range(-700f, 700f), UnityEngine.Random.Range(-700, 700)));
                        clouds[i].particleSystem.startColor = new Color(clouds[i].particleSystem.startColor.r, clouds[i].particleSystem.startColor.g, clouds[i].particleSystem.startColor.b, 1f);
                        clouds[i].particleSystem.startLifetime = 50;
                        clouds[i].transform.localScale = new Vector3(15, 15, 15);
                    }
                    catch (Exception e) { throw e; } }
            }
            else{
                foreach (GameObject cloud in clouds)
                {
                    float randomMove = UnityEngine.Random.Range(0.01f, 0.02f);
                    try
                    {
                        if (Application.loadedLevel == 2) { cloud.transform.position = new Vector3(-9999, -9999, -9999); }
                        cloud.transform.position += new Vector3(randomMove, randomMove - 0.015f, randomMove);
                        cloud.transform.localScale *= 1 + randomMove - 0.015f;
                        if (cloud.transform.position.x > 700) { cloud.transform.position = new Vector3(-700, cloud.transform.position.y, cloud.transform.position.z); }
                        if (cloud.transform.position.z > 700) { cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, -700); }
                    }
                    catch (Exception c) { throw c; Debug.Log("Didn't Moved!"); }
                }
            }
        }
        
    }




}
