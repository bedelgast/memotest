using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Runtime;
using UnityEngine.SceneManagement;

public class Controlador : MonoBehaviour
{
    /*Basado en el tutorial de como crear un juego de la memoria disponible en www.youtube.com/playlist?list=PLZhNP5qJ2IA2DA4bzDyxFMs8yogVQSrjW
    Script utilizado por el controlador para ejecutar las acciones del juego. Las "fotos" serian las imagenes utilizadas
    en el juego de la memoria. Cuando una "foto" se utiliza en el juego entonces se la llama "monumento" ya que el objetivo
    del juego es hallar todos los pares de monumentos. La estructura del juego es basicamente ir seleccionando objetos
    de listas y comparar si son el mismo. En caso positivo se suma un punto y en caso negativo se vuelve a dar vuelta
    esa pieza y el usuario tiene un nuevo intento. Al encontrar todos los pares de piezas el juego termina*/

    public Sprite dorso; //dorso de las piezas del juego
    public Sprite[] fotos; //arreglo que contiene todas las fotos disponibles para utilizar en el juego
    public List<Button> botones = new List<Button>(); //lista con los componentes Botones de las piezas
    public List<Sprite> monumentos = new List<Sprite>(); //lista de las fotos que se utilizaran en el partido actual
    private int cant_piezas; //para cambiar la cantidad de piezas hay que editar el script "CrearPiezas"
    bool primerIntento, segunIntento;
    string monumentoPrimerIntento, monumentoSegunIntento; //almacenan el nombre del archivo del monumento seleccionado
    int indicePrimerIntento, indiceSegunIntento; //almacena el indice en la lista de botones de cada intento
    int intentos_correctos, intentos_juego; //"_correctos" almacena los intentos correctos que hizo el jugador. "_juego" se calcula segun la cantidad de piezas. Cuando los dos son iguales significa que el jugador descubrio todas las piezas
    
    void Awake()
    {
        fotos = Resources.LoadAll<Sprite>("Monumentos"); //carga todas las imagenes en Resources/Monumentos
    }

    void Start()
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("pieza"); //identifica todas las piezas que se crearon con el script CrearPiezas
        cant_piezas = objetos.Length;
        intentos_juego = objetos.Length / 2;
        IdentificarBotones(objetos);
        ActivarBotones();
        List<int> lista = SeleccionarMonumentos();
        AgregarMonumentos(lista);
        MezclarPiezas(monumentos);
    }

    //este metodo lo que hace es identificar el componente Buttom de cada pieza, almacenarlo en una lista y de paso (ya que tamo) asignar la foto del dorso de la pieza
    void IdentificarBotones(GameObject[] parametro)
    {
        for (int i =0; i < parametro.Length; i++)
        {
            botones.Add(parametro[i].GetComponent<Button>());
            botones[i].image.sprite = dorso;
        }
        
    }

    /*este metodo surge de una particularidad en los requerimentos. se solicitó que en cada partido las fotos vayan variando,
    por ejemplo: en el archivo de imagenes existen 12 fotos, por ende el juego admite un maximo de 24 piezas. en una
    variante del juego se necesitaban 18 piezas, con lo cual este metodo se aplicaba para seleccionar aleatoriamente
    9 de las 12 fotos disponibles y asi variar los monumentos mostrados en cada partido*/
    List<int> SeleccionarMonumentos()
    {
        List<int> aux = new List<int>();
        for (int i = 0; i < cant_piezas/2; i++)
        {
            int num = Random.Range(0, fotos.Length);
            bool band = true;
            while (band)
            {
                if (aux.Contains(num))
                {
                    num = Random.Range(0, fotos.Length);
                }
                else
                {
                    aux.Add(num);
                    band = false;
                }
            }
        }
        return aux;
    }

    
    void AgregarMonumentos(List<int> indices)
    {
        int control = 0;
        for (int i = 0; i < cant_piezas; i++)
        {
            if (control == indices.Count())
            {
                control = 0;
            }
            monumentos.Add(fotos[indices[control]]);
            control++;
        }
    }

    void ActivarBotones()
    {
        foreach (Button boton in botones)
        {
            boton.onClick.AddListener(SeleccionarPieza);
        }
    }

    public void SeleccionarPieza()
    {
        string nombre = EventSystem.current.currentSelectedGameObject.name;

        if (!primerIntento)
        {
            primerIntento = true;
            indicePrimerIntento = int.Parse(nombre);
            botones[indicePrimerIntento].image.sprite = monumentos[indicePrimerIntento];
            monumentoPrimerIntento = monumentos[indicePrimerIntento].name;
        }
        else if (!segunIntento && (int.Parse(nombre) != indicePrimerIntento))
        {
            segunIntento = true;
            indiceSegunIntento = int.Parse(nombre);
            botones[indiceSegunIntento].image.sprite = monumentos[indiceSegunIntento];
            monumentoSegunIntento = monumentos[indiceSegunIntento].name;
            //cont_intentos++; si se quiere contar los intentos hay que agregar esta variable como atributo
            StartCoroutine(VerificarIntento());
        }
    }

    IEnumerator VerificarIntento()
    {
        yield return new WaitForSeconds(1f);
        if (monumentoPrimerIntento == monumentoSegunIntento)
        {
            botones[indicePrimerIntento].enabled = false;
            botones[indiceSegunIntento].enabled = false;
            VerificarFin();
        }
        else
        {
            botones[indicePrimerIntento].image.sprite = dorso;
            botones[indiceSegunIntento].image.sprite = dorso;
        }
        primerIntento = false;
        segunIntento = false;
    }

    void VerificarFin()
    {
        intentos_correctos++;
        if (intentos_correctos == intentos_juego)
        {
            Invoke("GameOver", 2f);
        }
    }

    void MezclarPiezas(List<Sprite> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            Sprite aux = lista[i];
            int aleatorio = Random.Range(i, lista.Count);
            lista[i] = lista[aleatorio];
            lista[aleatorio] = aux;
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("final");
    }

}
