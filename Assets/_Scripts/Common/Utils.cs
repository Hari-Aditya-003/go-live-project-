using UnityEngine;
using System.Collections.Generic;
using System;
using System.Globalization;
//using GameConsts;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

//This class is useful for most of common tasks which we are going to use in scripts
public static class Utils
{
	/// <summary>
	/// Gets or Adds a component of Type T.
	/// </summary>
	/// <returns>The or add component.</returns>
	/// <param name="child">Child.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T GetOrAddComponent<T> (this Component child) where T: Component
	{
		T result = child.GetComponent<T>();
		if(result == null)
			result = child.gameObject.AddComponent<T>();
		return result;
	}

	/// <summary>
	/// Actives or Deactive the gameObject which has called this function.
	/// </summary>
	/// <param name="child">Child.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void ActiveOrDeactive<T> (this Component child) where T: Component
	{
		child.gameObject.SetActive (!child.gameObject.activeSelf);
	}

	/// <summary>
	/// Plays the animation if not playing.
	/// </summary>
	/// <param name="child">Child.</param>
	/// <param name="animName">Animation name.</param>
	public static void PlayAnimationIfNotPlaying (this Component child, string animName)
	{
		if(!child.GetComponent<Animation>().IsPlaying (animName))
			child.GetComponent<Animation>().CrossFade (animName);
	}
	
	public static string ToTitleCase(this string title)
	{
		title = title.Replace("-", " ");
		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower()); 
	}
	public static string ToTitleCaseNoSpace(this string title)
	{
		title = title.Replace("-", " ");
		CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower()); 
		title = title.Replace(" ", "");
		return title;
	}

	/// <summary>
	/// Finds the child gameObject with the name. It searches all children and sub-children, 
	/// if It finds the gameObject it returns or it retuns null.
	/// checkDisabled is the bool value to check in  the disabled children as well.
	/// </summary>
	/// <returns>The child game object.</returns>
	/// <param name="parent">Parent.</param>
	/// <param name="childName">Child name.</param>
	/// <param name="checkDisabled">If set to <c>true</c> check disabled.</param>
	public static GameObject FindChildGameObject(this Transform parent, string childName, bool checkDisabled)
	{
		if(parent.name.Equals (childName)) return parent.gameObject;
		Transform[] allChildTransforms = parent.GetComponentsInChildren<Transform>(checkDisabled);

		foreach(Transform tr in allChildTransforms)
		{
			if(tr.name == childName) return tr.gameObject;
		}
		return null;
	}
    

    /// <summary>
    /// RayCasts to the distance mentioned and return the transform if hits.
    /// </summary>
    /// <returns>The ray cast tranasfom.</returns>
    /// <param name="tr">Tr.</param>
    /// <param name="dir">Dir.</param>
    /// <param name="dis">Dis.</param>
    public static Transform GetRayCastTranasfom(this Transform tr, Vector3 dir, float dis)
	{
		RaycastHit hit;
		Vector3 from = tr.position + Vector3.up;
		Debug.DrawRay (from, dir * dis, Color.green);
		if(Physics.Raycast(from, dir, out hit, dis))
		{
			if(hit.transform != null)
				return hit.transform;
		}
		return null;
	}

    public static void ClampAngles(this Component child, Vector2 xClamp, Vector2 yClamp, Vector2 zClamp, float inertiaSpeed)
	{
		Vector3 currentAngle = child.transform.eulerAngles;

		currentAngle.x = ClampAngle (currentAngle.x, xClamp.x, xClamp.y);
		currentAngle.y = ClampAngle (currentAngle.y, yClamp.x, yClamp.y);
		currentAngle.z = ClampAngle (currentAngle.z, zClamp.x, zClamp.y);

		//child.transform.rotation = Quaternion.Euler (currentAngle);
		child.transform.rotation = Quaternion.Lerp (child.transform.rotation, Quaternion.Euler (currentAngle), Time.deltaTime * inertiaSpeed);
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if(angle > 180f) angle -= 360f;
		if(angle < -360f) angle += 360f;
		if(angle > 360f) angle -= 360f;
		
		return Mathf.Clamp (angle, min, max);
	}

    static public bool ColorCompare(Color a, Color b, int diff)
    {
        return (int)(Mathf.Abs(a.r - b.r) * 100) < diff &&
            (int)(Mathf.Abs(a.g - b.g) * 100) < diff &&
            (int)(Mathf.Abs(a.b - b.b) * 100) < diff;
    }

    static public bool ColorCompare(Color a, Color b)
    {
        return ((int)(a.r * 100.0f) == (int)(b.r * 100.0f)) &&
            ((int)(a.g * 100.0f) == (int)(b.g * 100.0f)) &&
            ((int)(a.b * 100.0f) == (int)(b.b * 100.0f));
    }

    /// <summary>
    /// Shuffles the generic list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        System.Random rnd = new System.Random();
        while (n > 1)
        {
            int k = (rnd.Next(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// returs true, Is Web request done
    /// </summary>
    /// <param name="www"></param>
    /// <returns></returns>
    public static bool IsWWWNotDone(this WWW www)
    {
        if(!string.IsNullOrEmpty(www.error))
            Debug.Log("WWW call to " + www.url + " returned an Error : " + www.error);
        else if(string.IsNullOrEmpty(www.text))
            Debug.Log("WWW call to " + www.url + " returned no text");
        return (!www.isDone || string.IsNullOrEmpty(www.text) || !string.IsNullOrEmpty(www.error));
    }

    /// <summary>
    /// Sets the alpha of UI Graphic elements
    /// </summary>
    /// <param name="graphic"></param>
    /// <param name="val"></param>
    public static void SetAlpha(this UnityEngine.UI.Graphic graphic, float val)
    {
        Color c = graphic.color;
        c.a = val;
        graphic.color = c;
    }

    /// <summary>
    /// Adds IEnumerable type to current one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <param name="source"></param>
    public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
    {
        if (target == null)
            throw new ArgumentNullException(target.ToString());
        if (source == null)
            throw new ArgumentNullException(source.ToString());
        foreach (var element in source)
            target.Add(element);
    }

    public static float CropDecimalsToThreeDecimals(float number)
    {
        int num = Utils.ConvertFloatToInt(number);
        return Utils.ConvertIntToFloat(num);
    }


    public static float ConvertIntToFloat(int number)
    {
        return number / 1000.0f;
    }
  
    public static int ConvertFloatToInt(float number)
    {
        return (int)(number * 1000.0f);
    }
    
    /// <summary>
    /// Returns true if object is prefab or model asset
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsPrefabOrAsset(GameObject obj)
    {
	    // works for prefabs and asset models
	    return obj.scene.rootCount == 0;
    }

    /// <summary>
    /// Triggers the Event Asynchronously(Direct callstack)
    /// </summary>
    /// <param name="gameEvent"></param>
    public static void EventAsync(EventManager.GameEvent gameEvent)
    {
#if LOG_EVENTS
        Debug.Log("<color=magenta> Add Event " + gameEvent + " </color>");
#endif
        EventManager.Instance.TriggerEvent(gameEvent);
    }

    /// <summary>
    /// Adds Event to queue and processed in Update
    /// </summary>
    /// <param name="gameEvent"></param>
    public static void EventSync(EventManager.GameEvent gameEvent)
    {
#if LOG_EVENTS
        Debug.Log("<color=yellow> Add Event " + gameEvent + " </color>");
#endif
        EventManager.Instance.QueueEvent(gameEvent);
    }

    /// <summary>
    /// Check internet availibility
    /// </summary>
    /// <returns></returns>
    public static bool IsNetReachable()
    {
        bool result = false;
        result = (Application.internetReachability != NetworkReachability.NotReachable);
        return result;
    }

    // Convert an object to a byte array
    public static byte[] ObjectToByteArray(System.Object obj)
    {
        if (obj == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }

    // Convert a byte array to an Object
    public static System.Object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        System.Object obj = (System.Object)binForm.Deserialize(memStream);

        return obj;
    }

	/// <summary>
	/// Parses string data and find value by given key
	/// </summary>
	/// <param name="key"></param>
	/// <param name="data"></param>
	/// <returns></returns>
	public static string GetValueFromString(string data, string key, char dataSeparator, char keySeparator)
	{
		string value = string.Empty;
		string[] elements = data.Split(dataSeparator);
		for (int i = 0; i < elements.Length; i++)
		{
			if (elements[i].Contains(key))
				value = elements[i].Split(keySeparator)[1];
		}

		return value;
	}

	public static float GetAngleFromVectorFloat(Vector3 dir)
	{
		dir = dir.normalized;
		float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if (n < 0) n += 360;
		return n;
	}

    /// <summary>
    /// Returns component after Copying all values in given object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comp"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();

        // Return if, type mis-match.
        if (type != other.GetType())
            return null;

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }

        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }

        return comp as T;
    }

}
