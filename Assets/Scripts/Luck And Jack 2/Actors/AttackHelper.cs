using UnityEngine;

public struct AttackHelper
{

    private readonly FlatVector _position;
    private readonly Team _team;
    private readonly float _range;
    private readonly float _angle;

    public AttackHelper(FlatVector position, Team team, float range, float angle)
    {
        _position = position;
        _team = team;
        _range = range;
        _angle = angle;
    }

    public void Attack(AttackAction action, bool onlyFirst = false)
    {
        var actors = GameObject.FindObjectsOfType<Actor>();

        foreach (var actor in actors)
        {
            if (actor.IsDead)
                continue;

            if (actor.Team == _team)
                continue;

            var actorPosition = (FlatVector)actor.transform.position;

            var distance = FlatVector.Distance(_position, actorPosition);

            if (distance > _range)
                continue;

            var actorDirection = actorPosition - _position;
            var angle = FlatVector.Angle(actorDirection, _position);

            if (angle > _angle)
                continue;

            if (action.Invoke(actor, actorDirection) && onlyFirst)
                return;
        }
    }

    public delegate bool AttackAction(Actor target, FlatVector direction);

}