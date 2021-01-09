    using UnityEngine;
using UnityEngine.EventSystems;

namespace Aaron.LabDefenseRebuild
{
    public class BuildManager : MonoBehaviour
    {
        GameManager gameManager;

        [HideInInspector] public string chosenTurretName;
        [HideInInspector] public TurretCube chosenTurretCube;

        public Color normalColor;
        //public readonly Color mouseOnColor;
        public Color chosenColor;

        public delegate void ModifyTurretHandler(Turret @new, int doshFluctuation);
        public event ModifyTurretHandler ModifyTurretEvent;

        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            chosenTurretCube = null;
            chosenTurretName = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool isCollider = Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    var selectedCube = hit.collider.GetComponent<TurretCube>();
                    if (selectedCube != chosenTurretCube)
                    {
                        if (chosenTurretCube != null)
                            chosenTurretCube.gameObject.GetComponent<MeshRenderer>().material.color = normalColor;
                        selectedCube.gameObject.GetComponent<MeshRenderer>().material.color = chosenColor;
                        chosenTurretCube = selectedCube;
                    }

                    if (chosenTurretName != null && chosenTurretCube.turret == null)
                    {
                        chosenTurretCube.BuildTurret(chosenTurretName);
                        chosenTurretCube.gameObject.GetComponent<MeshRenderer>().material.color = normalColor;
                        chosenTurretName = null;
                    }
                    else if (chosenTurretCube.turret != null)
                    {
                        gameManager.viewSoloGame.ShowUpgradeUI(chosenTurretCube.turret);
                    }
                }
            }
        }

        public void ChooseTurret(string s)
        {
            chosenTurretName = s;
        }

        public void ModifyTurret(Turret @new, int cashFluctuation)
        {
            ModifyTurretEvent?.Invoke(@new, cashFluctuation);
        }
    }
}