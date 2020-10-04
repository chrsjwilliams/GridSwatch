using UnityEngine;

public class OnTileEnteredEvent : GameEvent
{
    public Entity entity;
    public OnTileEnteredEvent(Entity e) { entity = e; }
}