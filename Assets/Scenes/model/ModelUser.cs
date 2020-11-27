using UnityEngine;
public class ModelUser : vitamin.ModelBase
{
    public string name = "我是ModelUser";
    public ModelUser(){
        
    }

    override public void initialize(){
        vitamin.Logger.Log("ModelUser initialize!");
    }
}