using UnityEngine;
using UnityEngine.SceneManagement;
public class test : MonoBehaviour
{
    public int MenuIndex;
    public void ToMenu()
    {
        SceneManager.LoadScene(MenuIndex);
    }
}
