using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSaver
{
    public static Dictionary<Collider, Character> charactersSaved = new Dictionary<Collider, Character>();

    public static Character GetCharacter(Collider collider)
    {
        if (!charactersSaved.ContainsKey(collider))
        {
            Character c = collider.GetComponent<Character>();
            if (c)
                charactersSaved.Add(collider, c);
            else 
                return null;
        }
        return charactersSaved[collider];
    }

    public static Dictionary<Collider, Transform> ringsTFSaved = new Dictionary<Collider, Transform>();

    public static Transform GetRingTF(Collider collider)
    {
        if (!ringsTFSaved.ContainsKey(collider))
        {
            Transform r = collider.transform.GetChild(0);
            if (r)
                ringsTFSaved.Add(collider, r);
            else
                return null;
        }
        return ringsTFSaved[collider];
    }

    public static Dictionary<Collider, IDamage> iDamagesSaved = new Dictionary<Collider, IDamage>();


    public static IDamage GetIDamage(Collider collider)
    {
        if (!iDamagesSaved.ContainsKey(collider))
        {
            IDamage i = collider.GetComponent<IDamage>();
            if (i != null)
                iDamagesSaved.Add(collider, i);
            else
                return null;
        }
        return iDamagesSaved[collider];
    }
} 
