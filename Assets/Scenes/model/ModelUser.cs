namespace game.model
{
    using UnityEngine;
    public class ModelUser : vitamin.ModelBase
    {
        public string name = "Kevin.Chen";
        public ModelUser()
        {

        }

        override public void initialize()
        {
            vitamin.Logger.Log("ModelUser initialize!");
        }
    }
}