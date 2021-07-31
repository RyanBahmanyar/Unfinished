using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that deals with the oddities of a beat-em-up camera angle.
/// </summary>
public static class PerspectiveUtilities
{
    /// <summary>
    /// How much objects move vertically vs horizontally.
    /// (In beat'em-ups, characters move more horizontally than they do vertically due to the perspective of the camera.)
    /// </summary>
    public static readonly Vector2 perspectiveRatio = new Vector2(1, 0.75f);

    /// <summary>
    /// Brings a vector into foreshortened space.
    /// </summary>
    public static Vector2 ForeshortenVector(Vector2 original)
    {
        return new Vector2(original.x * perspectiveRatio.x, original.y * perspectiveRatio.y);
    }

    /// <summary>
    /// Brings a vector out of foreshortened space.
    /// </summary>
    public static Vector2 UnForeshortenVector(Vector2 foreshortened)
    {
        return new Vector2(foreshortened.x / perspectiveRatio.x, foreshortened.y / perspectiveRatio.y);
    }

    /// <summary>
    /// Gives a random vector in the foreshortened space that is in a circle with a radius of 1.
    /// </summary>
    public static Vector2 GetRandomVectorPerspectiveEllipse(bool randomMagnitude = true)
    {
        float angle = Random.Range(0f, 2f*Mathf.PI);
        float magnitude = randomMagnitude ? Random.value : 1f;

        Vector2 normalizedVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        return ForeshortenVector(normalizedVector * magnitude);
    }

    /// <summary>
    /// Measures the distance between two points in the foreshortened space.
    /// </summary>
    public static float GetForeshortenedDistance(Vector2 vec1, Vector2 vec2)
    {
        return Vector2.Distance(UnForeshortenVector(vec1), UnForeshortenVector(vec2));
    }
}
