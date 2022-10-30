using UnityEngine;

public interface IMorphable
{
    public Sprite StartSprite
    {
        get => StartSprite;
    }
    public void Morph(Sprite morphSprite, float morphTime);
    public void Unmorph();
}
