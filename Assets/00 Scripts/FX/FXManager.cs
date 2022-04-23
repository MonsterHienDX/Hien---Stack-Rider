using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    private FX_Break _FX_Break;

    private List<FX_Break> _FX_Breakes;

    private static FXManager _instance;
    private AudioManager _audio;

    public FX_Break _FX_Prefabs;
    public static FXManager instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        _audio = FindObjectOfType<AudioManager>();
    }

    public void Start()
    {
        this._FX_Breakes = new System.Collections.Generic.List<FX_Break>();
    }
    public void Play(Vector3 position, Material material)
    {
        ShowFX(position, material);
    }

    void ShowFX(Vector3 position, Material material = null)
    {
        FX_Break _FX_Break = this.GetFX();
        ParticleSystemRenderer p = _FX_Break.gameObject.GetComponentInChildren<ParticleSystemRenderer>();
        ChangeMaterialFX(material, p);

        _FX_Break.transform.position = position;
        _FX_Break.gameObject.SetActive(value: true);
        _FX_Break.CanUse = false;

    }

    private FX_Break GetFX()
    {
        for (int i = 0; i < _FX_Breakes.Count; i++)
        {
            if (_FX_Breakes[i].CanUse)
                return _FX_Breakes[i];
        }

        FX_Break _FX_Break = Instantiate<FX_Break>(_FX_Prefabs, this.transform);
        _FX_Breakes.Add(_FX_Break);
        return _FX_Break;
    }

    public void ChangeMaterialFX(Material material, ParticleSystemRenderer p)
    {
        p.material = material;
    }

}
