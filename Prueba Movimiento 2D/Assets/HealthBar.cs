using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Renderer shader;
    // Start is called before the first frame update
    void Start()
    {
        shader = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var characterControllerNonUnity = GameObject.Find("DrawCharacter").GetComponent<CharacterControllerNonUnity>();
        shader.sharedMaterial.SetFloat("_Progress", (characterControllerNonUnity.life / 100));
    }
}
