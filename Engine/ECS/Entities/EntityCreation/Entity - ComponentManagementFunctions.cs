namespace Engine.ECS.Entities.EntityCreation;

public abstract partial class Entity
{
    public void ChangeKind(EntityKind newKind)
    {
        EntityManager.RemoveEntityFromSubList(this);
        EntityKind = newKind;
        EntityManager.AddEntityToSubList(this);
    }

    public void SetDrawOrder(int newDrawOrder) =>
        DrawOrder = newDrawOrder;
}