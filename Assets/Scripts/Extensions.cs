using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Extensions
{
	static class Extensions
	{
		public static Vector2 Direction(Vector2 source, Vector2 target)
		{
			return target - source;
		}

		public static Vector2 Direction(this Transform source, Vector2 target)
		{
			return Direction(source.position, target);
		}

		public static Vector2 Direction(this Transform source, Transform target)
		{
			return Direction(source.position, target.position);
		}

		public static void LookAt2D(this Transform transform, Vector2 target)
		{
			transform.right = Direction(transform.position, target);
		}

		public static void LookAt2D(this Transform transform, Transform target)
		{
			LookAt2D(transform, target.position);
		}

		public static Vector2 Rotate2DDeg(this Vector2 vector, float degrees)
		{
			return vector.Rotate2DRad(degrees * Mathf.Deg2Rad);
		}

		public static Vector2 Rotate2DRad(this Vector2 vector, float radians)
		{
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);
			return new((cos * vector.x) - (sin * vector.y), (sin * vector.x) + (cos * vector.y));
		}

		public static Vector2 DirectionFromDeg(float degrees)
		{
			return DirectionFromRad(degrees * Mathf.Deg2Rad);
		}

		public static Vector2 DirectionFromRad(float radians)
		{
			return new(Mathf.Cos(radians), Mathf.Sin(radians));
		}

		public static float DegFromDirection(this Vector2 direction)
		{
			return Vector2.SignedAngle(Vector2.right, direction);
		}

		public static float RadFromDirection(this Vector2 direction)
		{
			return DegFromDirection(direction) * Mathf.Deg2Rad;
		}

		public static bool Contains(this LayerMask mask, int layer)
		{
			return mask == (mask | (1 << layer));
		}

		public static int LayerIndex(this LayerMask mask)
		{
			return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
		}

		public static Vector2 GetAverageContactPoint(this Collision2D collision)
		{
			var points = new ContactPoint2D[collision.contactCount];
			collision.GetContacts(points);
			Vector2 averagePoint = Vector2.zero;
			foreach (var point in points)
				averagePoint += point.point;
			averagePoint /= points.Length;
			return averagePoint;
		}

		public static float GetAverageImpulse(this Collision2D collision)
		{
			var points = new ContactPoint2D[collision.contactCount];
			collision.GetContacts(points);
			float averageImpulse = 0;
			foreach (var point in points)
				averageImpulse += point.normalImpulse;
			averageImpulse /= points.Length;
			return averageImpulse;
		}

		public static T GetRandom<T>(this IList<T> source) => source[Random.Range(0, source.Count)];

		public static bool TryRoll(float chance)
		{
			return chance >= 1f || Random.value <= chance;
		}

		public static bool GetRandomBool()
		{
			return Random.Range(0, 2) == 1;
		}

		public static float GetRandomPolarity(float multiplier = 1f)
		{
			return GetRandomBool() ? multiplier : -multiplier;
		}

		public static Vector2 GetRandomDirection2DRad(float rangeRadians)
		{
			return new(Mathf.Cos(rangeRadians), Mathf.Sin(rangeRadians));
		}

		public static Vector2 GetRandomDirection2DDeg(float rangeDegrees)
		{
			return GetRandomDirection2DRad(rangeDegrees * Mathf.Deg2Rad);
		}

		public static Transform[] GetTransformsInChildrenExcludingParent(this Transform parent)
		{
			return parent.GetComponentsInChildren<Transform>().Where(o => o != parent).ToArray();
		}

		public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> collection, Func<TSource, TKey> selector) where TKey : IComparable<TKey>
		{
			TSource min = collection.FirstOrDefault();
			if (!min.Equals(default(TSource)))
			{
				TKey minVal = selector(min);
				foreach (var item in collection)
				{
					TKey val = selector(item);
					if (val.CompareTo(minVal) < 0)
					{
						min = item;
						minVal = val;
					}
				}
			}
			return min;
		}

		/// <summary>
		/// Similar to LINQ's TakeWhile, but the <paramref name="predicate"/> is inverted and the failed element is also returned.
		/// </summary>
		public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			foreach (T item in source)
			{
				yield return item;
				if (predicate(item))
					yield break;
			}
		}

		public static Vector2 Average(this IEnumerable<Vector2> source)
		{
			uint count = 0;
			Vector2 sum = Vector2.zero;
			foreach (var item in source)
			{
				count++;
				sum += item;
			}
			if (count == 0)
				return Vector2.zero;
			return sum / count;
		}

		public static Vector3 Average(this IEnumerable<Vector3> source)
		{
			uint count = 0;
			Vector3 sum = Vector3.zero;
			foreach (var item in source)
			{
				count++;
				sum += item;
			}
			if (count == 0)
				return Vector3.zero;
			return sum / count;
		}

		public static float TriangleWave(float f) => 2 * Mathf.Abs(f - Mathf.Floor(f + 0.5f)) - 1;

		public static float SquareWave(float f) => 2 * (2 * Mathf.Floor(f) - Mathf.Floor(2 * f)) + 1;

		public static VC GetVolumeComponent<VC>(this VolumeProfile profile) where VC : VolumeComponent
		{
			return profile.components.Find(vc => vc is VC) as VC;
		}

		/// <param name="seconds">Seconds to convert</param>
		/// <returns>Time formatted as "MM:SS".</returns>
		public static string TimeToMMSS(int seconds) => $"{seconds / 60:00}:{seconds % 60:00}";
	}
}
