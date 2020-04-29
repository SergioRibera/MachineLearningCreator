using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Languaje", menuName = "Idioma")]
public partial class IdiomaScriptableObject : ScriptableObject
{
    public string LanguajeName;
    public StringMatrix Contenido;

    public static implicit operator Idioma(IdiomaScriptableObject i)
    {
        Idioma idioma = new Idioma();
        idioma.LanguajeName = i.LanguajeName;
        idioma.Contenido = i.Contenido;
        return idioma;
    }
}