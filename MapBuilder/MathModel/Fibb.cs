using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilder.MathModel
{
    class Fibb : Model
    {
        public enum ModelChanges
        {
            Double,Triple,Steep,Base
        }

        List<int> baseModel = new List<int>() {1,1,2,3,5,8,13,21,34};
        public List<List<int>> Models { get; } = new List<List<int>>();

        public Fibb()
        {
            for (int i = 0; i < Enum.GetNames(typeof(ModelChanges)).Length; i++)
            {
                Models.Add(AlterModel((ModelChanges)i));
            }
        }


        public List<int> AlterModel(ModelChanges modelChanges)
        {
            List<int> alteredModel = new List<int>();

            switch (modelChanges)
            {
                case ModelChanges.Base:
                    alteredModel = baseModel;
                    break;

                case ModelChanges.Double:
                    for (int i = 0; i < baseModel.Count; i++)
                    {
                        alteredModel.Add(baseModel[i]);
                        alteredModel.Add(baseModel[i]);
                    }
                    break;

                case ModelChanges.Triple:
                    for (int i = 0; i < baseModel.Count; i++)
                    {
                        alteredModel.Add(baseModel[i]);
                    }
                    break;

                case ModelChanges.Steep:
                    for (int i = 0; i < baseModel.Count; i = i + 2)
                    {
                        alteredModel.Add(baseModel[i]);
                    }
                    break;
                    /*
                case ModelChanges.Giant:
                    for (int i = 0; i < baseModel.Count; i++)
                    {
                        alteredModel.Add(baseModel[i]);
                        alteredModel.Add(baseModel[i]);
                        alteredModel.Add(baseModel[i]);
                        alteredModel.Add(baseModel[i]);
                        alteredModel.Add(baseModel[i]);
                        alteredModel.Add(baseModel[i]);
                        alteredModel.Add(baseModel[i]);
                        alteredModel.Add(baseModel[i]);
                    }
                    break;
                    */
            }

            return alteredModel;
        }
    }
}
