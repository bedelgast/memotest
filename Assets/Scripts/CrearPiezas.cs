using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearPiezas : MonoBehaviour
{
    /*Basado en el tutorial de como crear un juego de la memoria disponible en www.youtube.com/playlist?list=PLZhNP5qJ2IA2DA4bzDyxFMs8yogVQSrjW
    Script utilizado por el controlador para crear las piezas del juego de la memoria. Las piezas son clones
    de un prefab disponible en la carpeta de Pregabs. Para cambiar la cantidad de piezas hay que cambiar el
    valor de la variable y actualizar las configuraciones del Grid en el Tablero adentro del Canvas para
    manetener la simetria*/

    public Transform tablero;
    public GameObject pz;
    int cantidad_piezas = 24;

    void Awake() //Este metodo se ejecuta antes de todo cuando se carga la escena
    {
        for (int i = 0; i < cantidad_piezas; i++)
        {
            GameObject pieza = Instantiate(pz);
            pieza.name = "" + i;
            pieza.transform.SetParent(tablero, false);
        } //Luego de crear una instancia de pieza setea su posicion al tablero como jerarquia superior

    }
}
