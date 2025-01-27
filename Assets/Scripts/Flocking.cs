﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flocking : MonoBehaviour
{

    public Vector3 baseRotation;

    [Range(0, 10)]
    public float maxSpeed = 1f;

    [Range(.1f, .5f)]
    public float maxForce = .03f;

    [Range(1, 10)]
    public float neighborhoodRadius = 3f;

    [Range(0, 3)]
    public float separationAmount = 1f;

    [Range(0, 3)]
    public float cohesionAmount = 1f;

    [Range(0, 3)]
    public float alignmentAmount = 1f;

    [Range(0, 3)]
    public float esquiveAmount = 1f;

    public Vector2 acceleration;
    public Vector2 velocity;
    public Vector2 esquive;

    private Vector2 Position {
        get {
            return gameObject.transform.position;
        }
        set {
            gameObject.transform.position = value;
        }
    }

    private void Start()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle) + baseRotation);
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void Update()
    {
        var boidColliders = Physics2D.OverlapCircleAll(Position, neighborhoodRadius);
        
        List<Flocking> listBoids = new List<Flocking>();
        foreach (Collider2D col in boidColliders)
        {
            var flock = col.GetComponent<Flocking>();
            if (flock != null)
            {
                listBoids.Add(flock);
            }else if(col.tag == "Mine" || col.tag == "Player")
            {
                esquive.x = Position.x - col.transform.position.x;
                esquive.y = Position.y - col.transform.position.y;
            }
            else
            {
                esquive = Vector2.zero;
            }
        }

        listBoids.Remove(this);
        Flock(listBoids);
        UpdateVelocity();
        UpdatePosition();
        UpdateRotation();
        WrapAround();
    }

    private void Flock(IEnumerable<Flocking> boids)
    {
        var alignment = Alignment(boids);
        var separation = Separation(boids);
        var cohesion = Cohesion(boids);

        acceleration = alignmentAmount * alignment + cohesionAmount * cohesion + separationAmount * separation + esquiveAmount * esquive;
    }

    public void UpdateVelocity()
    {
        velocity += acceleration;
        velocity = LimitMagnitude(velocity, maxSpeed);
    }

    public void Collision()
    {
        
    }

    private void UpdatePosition()
    {
        Position += velocity * Time.deltaTime;
    }

    private void UpdateRotation()
    {
        var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle) + baseRotation);
    }

    private Vector2 Alignment(IEnumerable<Flocking> boids)
    {
        var velocity = Vector2.zero;
        if (!boids.Any()) return velocity;

        foreach (var boid in boids)
        {
            velocity += boid.velocity;
        }
        velocity /= boids.Count();

        var steer = Steer(velocity.normalized * maxSpeed);
        return steer;
    }

    private Vector2 Cohesion(IEnumerable<Flocking> boids)
    {
        if (!boids.Any()) return Vector2.zero;

        var sumPositions = Vector2.zero;
        foreach (var boid in boids)
        {
            sumPositions += boid.Position;
        }
        var average = sumPositions / boids.Count();
        var direction = average - Position;

        var steer = Steer(direction.normalized * maxSpeed);
        return steer;
    }

    private Vector2 Separation(IEnumerable<Flocking> boids)
    {
        var direction = Vector2.zero;
        boids = boids.Where(o => DistanceTo(o) <= neighborhoodRadius / 2);
        if (!boids.Any()) return direction;

        foreach (var boid in boids)
        {
            var difference = Position - boid.Position;
            direction += difference.normalized / difference.magnitude;
        }
        direction /= boids.Count();

        var steer = Steer(direction.normalized * maxSpeed);
        return steer;
    }

    private Vector2 Steer(Vector2 desired)
    {
        var steer = desired - velocity;
        steer = LimitMagnitude(steer, maxForce);

        return steer;
    }

    private float DistanceTo(Flocking boid)
    {
        return Vector3.Distance(boid.transform.position, Position);
    }

    private Vector2 LimitMagnitude(Vector2 baseVector, float maxMagnitude)
    {
        if (baseVector.sqrMagnitude > maxMagnitude * maxMagnitude)
        {
            baseVector = baseVector.normalized * maxMagnitude;
        }
        return baseVector;
    }

    private void WrapAround()
    {

        if (Camera.main.WorldToScreenPoint(Position).x > Screen.width)
        {
            Position = new Vector2(-Position.x + 0.2f, Position.y);
        }
        if (Camera.main.WorldToScreenPoint(Position).x < 0)
        {
            Position = new Vector2(Mathf.Abs(Position.x) - 0.2f, Position.y);
        }
        if (Camera.main.WorldToScreenPoint(Position).y > Screen.height)
        {
            Position = new Vector2(Position.x, -Position.y + 0.2f);
        }
        if (Camera.main.WorldToScreenPoint(Position).y < 0)
        {
            Position = new Vector2(Position.x, Mathf.Abs(Position.y) - 0.2f);
        }
    }
}