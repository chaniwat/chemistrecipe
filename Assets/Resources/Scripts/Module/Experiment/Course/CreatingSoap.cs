using chemistrecipe.experiment.model;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace chemistrecipe.experiment.course
{
    class CreatingSoap : MonoBehaviour, ICourse
    {
        #region Unity Scene GameObject

        public GameObject time_text_obj;
        public GameObject e1_mat_lists_obj;
        public GameObject e2_mat_lists_obj;
        public GameObject e3_mat_lists_obj;

        private Text time_text;
        private Text e1_mat_lists;
        private Text e2_mat_lists;
        private Text e3_mat_lists;

        #endregion

        #region Course Settings

        // Course time
        private float currentCorseRunTime = 0f;

        // Materials
        private const string WATER = "Water";
        private const string SODIUM_HYDROXIDE = "Sodium Hydroxide";
        private const string COCONUT_OIL = "Coconut Oil";
        private const string MIXED_WATER_SODIUM_HYDROXIDE = "Water + Sodium Hydroxide";
        private const string SOAP_LIQUID = "Soap (liquid)";

        // Equipments
        private FillableEquipment e1, e2, e3;

        // Water heat status
        private bool waterHeating = false;

        #endregion

        public void Start()
        {
            #region Unity Setup

            time_text = time_text_obj.GetComponent<Text>();
            e1_mat_lists = e1_mat_lists_obj.GetComponent<Text>();
            e2_mat_lists = e2_mat_lists_obj.GetComponent<Text>();
            e3_mat_lists = e3_mat_lists_obj.GetComponent<Text>();

            #endregion

            #region Course Setup (equipments & materials)

            // Beaker 1000mL with water 150mL
            e1 = new Beaker1000mL();
            e1.fill(new model.Material(WATER, MaterialType.LIQUID), new Volume(150, Volume.Metric.mL));
            e1.StirFunc = () =>
            {
                HandleStirBeaker_1(e1);
            };
            e1.UpdateFunc = () =>
            {
                HandleUpdateBeaker_1(e1);
            };

            // Beaker 1000mL with sodium_hydroxide 15g
            e2 = new Beaker1000mL();
            e2.fill(new model.Material(SODIUM_HYDROXIDE, MaterialType.POWDER), new Volume(15, Volume.Metric.g));
            e2.StirFunc = () =>
            {
                HandleStirBeaker_1(e2);
            };
            e2.UpdateFunc = () =>
            {
                HandleUpdateBeaker_1(e2);
            };

            // Beaker 1000mL with coconut_oil 450mL
            e3 = new Beaker1000mL();
            e3.fill(new model.Material(COCONUT_OIL, MaterialType.LIQUID), new Volume(450, Volume.Metric.mL));
            e3.StirFunc = () =>
            {
                HandleStirBeaker_1(e3);
            };
            e3.UpdateFunc = () =>
            {
                HandleUpdateBeaker_1(e3);
            };

            #endregion
        }

        public void Update()
        {
            #region GUI Update

            // Update run time
            currentCorseRunTime += Time.deltaTime;
            time_text.text = "Time: " + currentCorseRunTime.ToString("0.0000");

            // e1 element list
            string e1_mat_lists_string = "";
            foreach(KeyValuePair<model.Material, Volume> pair in e1.materials)
            {
                Volume vol = pair.Value;
                e1_mat_lists_string += String.Format("{0} : {1} {2} {3}c\n", pair.Key.name, vol.volume, vol.metric, vol.tempature);
            }
            e1_mat_lists.text = e1_mat_lists_string;

            // e2 element list
            string e2_mat_lists_string = "";
            foreach (KeyValuePair<model.Material, Volume> pair in e2.materials)
            {
                Volume vol = pair.Value;
                e2_mat_lists_string += String.Format("{0} : {1} {2} {3}c\n", pair.Key.name, vol.volume, vol.metric, vol.tempature);
            }
            e2_mat_lists.text = e2_mat_lists_string;

            // e3 element list
            string e3_mat_lists_string = "";
            foreach (KeyValuePair<model.Material, Volume> pair in e3.materials)
            {
                Volume vol = pair.Value;
                e3_mat_lists_string += String.Format("{0} : {1} {2} {3}c\n", pair.Key.name, vol.volume, vol.metric, vol.tempature);
            }
            e3_mat_lists.text = e3_mat_lists_string;

            #endregion

            #region Course Update

            e1.Update();
            e2.Update();
            e3.Update();

            #endregion
        }

        #region Command Handler

        public void e1Stir()
        {
            e1.Stir();
        }

        public void e2Stir()
        {
            e2.Stir();
        }

        public void e3Stir()
        {
            e3.Stir();
        }

        public void e1PourToe2()
        {
            e1.pour(e2);
        }

        public void e1PourToe3()
        {
            e1.pour(e3);
        }

        public void e2PourToe1()
        {
            e2.pour(e1);
        }

        public void e2PourToe3()
        {
            e2.pour(e3);
        }

        public void e3PourToe1()
        {
            e3.pour(e1);
        }

        public void e3PourToe2()
        {
            e3.pour(e2);
        }

        #endregion

        #region QUICK REGION

        public void HandleUpdateBeaker_1(FillableEquipment equipment)
        {
            #region If Have Water + Sodium Hydroxide and its Temp > 25c, cool it down over time

            if (equipment.containMaterial(MIXED_WATER_SODIUM_HYDROXIDE))
            {
                Volume vol = equipment.materials[equipment.getMaterial(MIXED_WATER_SODIUM_HYDROXIDE)];
                vol.tempature -= 0.25f * Time.deltaTime;
            }

            #endregion
        }

        public void HandleStirBeaker_1(FillableEquipment equipment)
        {
            #region If equipment contain Sodium Hydroxide and Water, mixed it!

            if (equipment.containMaterial(SODIUM_HYDROXIDE) && equipment.containMaterial(WATER))
            {
                // Mixed material
                if (!equipment.containMaterial(MIXED_WATER_SODIUM_HYDROXIDE))
                {
                    equipment.fill(new model.Material(MIXED_WATER_SODIUM_HYDROXIDE, MaterialType.LIQUID), new Volume(11f, 26f, Volume.Metric.mL));
                }
                else
                {
                    Volume vol = equipment.materials[equipment.getMaterial(MIXED_WATER_SODIUM_HYDROXIDE)];
                    vol.volume += 11f;
                    vol.tempature += 1f;
                }

                // Reduce material volume
                //
                // Sodium Hydroxide
                Volume sh = equipment.getVolumeOfMaterial(SODIUM_HYDROXIDE);
                sh.volume -= 1f;
                if (sh.volume <= 0)
                {
                    equipment.removeMaterial(SODIUM_HYDROXIDE);
                }

                // Water
                Volume wa = equipment.getVolumeOfMaterial(WATER);
                wa.volume -= 10f;
                if (wa.volume <= 0)
                {
                    equipment.removeMaterial(WATER);
                }
            }

            #endregion

            #region If Have Water + Sodium Hydroxide with Coconut Oil

            if (equipment.containMaterial(MIXED_WATER_SODIUM_HYDROXIDE) && equipment.containMaterial(COCONUT_OIL))
            {
                // Mixed material
                if (!equipment.containMaterial(SOAP_LIQUID))
                {
                    equipment.fill(new model.Material(SOAP_LIQUID, MaterialType.LIQUID), new Volume(29.5f, Volume.Metric.mL));
                }
                else
                {
                    equipment.materials[equipment.getMaterial(SOAP_LIQUID)].volume += 11f;
                }

                // Reduce material volume
                //
                // Sodium Hydroxide
                Volume sh = equipment.getVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
                sh.volume -= 5f;
                if (sh.volume <= 0)
                {
                    equipment.removeMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
                }

                // Water
                Volume wa = equipment.getVolumeOfMaterial(COCONUT_OIL);
                wa.volume -= 13f;
                if (wa.volume <= 0)
                {
                    equipment.removeMaterial(COCONUT_OIL);
                }
            }

            #endregion
        }

        #endregion

    }
}
