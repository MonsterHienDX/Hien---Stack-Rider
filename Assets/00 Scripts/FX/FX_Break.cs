using System;
using UnityEngine;

public class FX_Break : MonoBehaviour
{
    public float _lifeTime = 0.8f;

    public ParticleSystem BallBreak;

    private bool _canUse;
    public bool CanUse
    { // This is a property.
        get
        {
            return _canUse;
        }

        set
        {
            _canUse = value; // Note the use of the implicit variable "value" here.
        }
    }

    public void Update()
    {
        float val_1 = UnityEngine.Time.deltaTime;
        val_1 = this._lifeTime - val_1;
        this._lifeTime = val_1;
        if (val_1 >= 0)
        {
            return;
        }

        Disable();
    }

    private void OnEnable()
    {
    }

    private void Disable()
    {
        CanUse = true;
        this.gameObject.SetActive(value: false);
        this._lifeTime = 0.8f;
    }

    public void startExplosion()
    {

    }
    public void SetConfetti(bool withConfetti)
    {
    }

}
