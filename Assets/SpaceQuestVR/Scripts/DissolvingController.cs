using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingController : MonoBehaviour
{
    public MeshRenderer mesh;
    public VisualEffect vfxGraph;
    float dissolveRate = 0.025f;
    float refreshRate = 0.025f;
    Material[] materials;

    void Awake()
    {
        if (mesh != null)
            materials = mesh.materials;
    }

    public void Disappear()
    {
        StartCoroutine(Dissolve());
    }

    public void Appear()
    {
        StartCoroutine(Condense());
    }

    IEnumerator Dissolve()
    {
        if (vfxGraph) vfxGraph.Play();
        if (materials.Length > 0)
        {
            float counter = 0f;
            while (materials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter = Mathf.Max(counter + dissolveRate, 1);
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

    IEnumerator Condense()
    {
        if (vfxGraph) vfxGraph.Play();
        if (materials.Length > 0)
        {
            float counter = 1f;
            materials[0].SetFloat("_DissolveAmount", counter);
            while (materials[0].GetFloat("_DissolveAmount") >= 0)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_DissolveAmount", counter);
                }
                counter = Mathf.Max(counter - dissolveRate, 0);
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
