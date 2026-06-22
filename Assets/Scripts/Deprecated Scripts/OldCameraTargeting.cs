/*
public class OldCameraTargeting : MonoBehaviour
{
    /// <summary>
    /// Checks if the <c>detectRadius</c> area has the same enemies
    /// </summary>
    /// <returns>true if it contains the same enemies, false otherwise</returns>
    private bool SameEnemiesCheck()
    {
        if (enemyIDs.Count != enemyObjects.Count)
            return false;

        for (int i = 0; i < enemyIDs.Count; i++)
        {
            if (!enemyObjects.ContainsKey((int)enemyIDs[i]))
                return false;
        }
        return true;
    }
    /// <summary>
    /// Checks if enemies are the same distance
    /// </summary>
    /// <returns></returns>
    private bool SameDistanceCheck()
	{
        ArrayList tempPrev = new ArrayList();
        bool sameDistances = true;

        if (enemiesInRadius.Count != prevEnemiesInRadius.Count)
            sameDistances = false;

        for (int i = 0; i < enemiesInRadius.Count; i++)
		{
            tempPrev.Add(enemiesInRadius[i]);
            GameObject enemy = (GameObject) enemiesInRadius[i];
            try
            {
                GameObject prevEnemy = (GameObject)prevEnemiesInRadius[i];

                if (enemy.transform.position != prevEnemy.transform.position)
                    sameDistances = false;
            }
            catch (System.Exception) { }
		}

        if (!sameDistances)
            prevEnemiesInRadius = tempPrev;
            

        return sameDistances;
	}
    /// <summary>
    /// Checks and gets enemy objects' information in the <c>detectRadius</c> area
    /// </summary>
    /// <returns>Enemy objects in the area</returns>
    private Dictionary<int, GameObject> GetEnemiesInPerimeter()
    {
        enemiesInRadius.Clear();
        enemyIDs.Clear();

        Dictionary<int, GameObject> enemiesInPerimeter = new Dictionary<int, GameObject>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
			{
                Collider cl = colliders[i];
				enemiesInPerimeter.Add(cl.GetInstanceID(), cl.gameObject);
                enemyIDs.Add(colliders[i].GetInstanceID());
                //If same enemies are near the player, don't add enemies to enemies in radius
                enemiesInRadius.Add(cl.gameObject);
            }
        }

        return enemiesInPerimeter;
    }
    /// <summary>
    /// Searches for the closest enemy to target if there are any based on <c>detectRadius</c>
    /// </summary>
    /// <returns>Gameobject of an enemy</returns>
    private GameObject ClosestEnemy()
	{
        GameObject closestObject;
        //Error handling for if there are no enemies close by
        try
		{
            if (!enemyObjects.TryGetValue((int)enemyIDs[0], out closestObject))
                return null;
        } catch (System.Exception)
		{
            return null;
		}

		float shortestDistance = Vector3.Distance(transform.position, closestObject.transform.position);
        float tempDistance;
        //Searching for enemy with the minimum distance to player
        for (int i = 1; i < enemyIDs.Count; i++)
		{
            if (!enemyObjects.TryGetValue((int)enemyIDs[i], out GameObject tempObject))
                return null;

            tempDistance = Vector3.Distance(transform.position, tempObject.transform.position);
            if (tempDistance < shortestDistance)
			{
                shortestDistance = tempDistance;
                closestObject = tempObject;
            }
		}

        return closestObject;
    private void PrintEnemyDistances(GameObject[] enemies)
	{
        for (int i = 0; i < enemies.Length; i++)
		{
            float distance = Vector3.Distance(enemies[i].transform.position, transform.position);
            Debug.Log("Enemy: " + enemies[i].name + "---Distance: " + distance + "---Index: " + i);
		}
	}
	}
}*/
