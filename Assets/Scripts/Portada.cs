using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portada : MonoBehaviour
{
    //Script utilizado por el controlador para implementar la navegacion entre las escenas del juego luego de hacer clic en los botones

    public void Jugar()
    {
        SceneManager.LoadScene("juego");
    }

    public void Volver()
    {
        SceneManager.LoadScene("portada");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
