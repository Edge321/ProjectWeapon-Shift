using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTargeting : MonoBehaviour
{
    public GameObject tpsCamera;
    public GameObject targetCamera;

    public CinemachineTargetGroup targetGroup;

    public bool showEnemyLockedOn = true;

	[Range(0.05f, 0.5f)]
    public float cameraSwitchTime = 0.2f;
    [Range(10f, 100f)]
    public float detectRadius = 50f;
    [Range(0.0001f, 3f)]
    public float checkEnemyTime = 0.5f;

    private bool tpsOff = false;

    private int currentEnemy = 0;

    private float holdFTime = 0;

    private GameObject enemyLocked;

    private GameObject[] enemiesInRadius;
	private void Awake()
	{
        InvokeRepeating("EnemyCheck", 0f, checkEnemyTime);
	}
	void Update()
    {
        //Determines the cameras behavior from the F key
        if (Input.GetKey(KeyCode.F))
            holdFTime += Time.deltaTime;
        else if (Input.GetKeyUp(KeyCode.F))
            ChangeCameraBehavior();
        else
            holdFTime = 0;
        //TODO: make player turn smoothly to the enemy, look at player movement script
        //TODO: only make player's y transformation be modified
        if (targetCamera.activeSelf)
            LookAtEnemy();
            
            
    }
    /// <summary>
    /// The player can press the F key to target/switch targets or hold F to un-target
    /// </summary>
    private void ChangeCameraBehavior()
	{
        //Switching between enemies or from third person to targeting
        if (holdFTime > 0 && holdFTime <= cameraSwitchTime)
		{
            TPStoTargetingCamera();
        }
        //Switching between cameras
        else if (holdFTime > cameraSwitchTime)
        {
            if (tpsOff)
                TargetingtoTPSCamera();
            else
                TPStoTargetingCamera();
        }
    }
    /// <summary>
    /// Switches player's camera from targeting an enemy to third person
    /// </summary>
    private void TargetingtoTPSCamera()
	{
        tpsCamera.SetActive(true);
        targetCamera.SetActive(false);
        tpsOff = false;
        enemiesInRadius = null;
        enemyLocked = null;
        currentEnemy = 0;
        targetGroup.m_Targets[1].target = null;
    }
    /// <summary>
    /// Checks every <c>checkEnemyTime</c> seconds if enemies too far away to lock on
    /// </summary>
    private void EnemyCheck()
    {
        if (GetNearbyEnemies().Count == 0)
            TargetingtoTPSCamera();
    }
    /// <summary>
    /// Switches player's camera from third person to targeting an enemy
    /// </summary>
    /// <param name="tempEnemies"></param>
    private void TPStoTargetingCamera()
	{
        ArrayList tempEnemies = GetNearbyEnemies();
        GameObject[] enemies = ConvertArrayList(tempEnemies);
        QuickSort(enemies, 0, enemies.Length - 1);
        //Makes sure an error does not occur if no enemies are around
        if (tempEnemies.Count > 0)
		{
            if (SameEnemiesCheck(enemies))
            {
                currentEnemy = (currentEnemy + 1) % enemiesInRadius.Length;
                enemyLocked = enemiesInRadius[currentEnemy];
            }
            else
            {
                enemiesInRadius = enemies;
                currentEnemy = 0;

                //Avoids enemyLocked not initialized error
                if (enemyLocked != null)
				{
                    //If the player moves to a target of index 1 of enemiesInRadius (for example),
                    //removes potential to target that enemy again since the enemy of
                    //index 1 will be the closest enemy
                    if (enemyLocked.GetInstanceID() == enemiesInRadius[currentEnemy].GetInstanceID())
                        currentEnemy = (currentEnemy + 1) % enemiesInRadius.Length;
                }
                
                enemyLocked = enemiesInRadius[currentEnemy];
            }

            targetGroup.m_Targets[1].target = enemyLocked.transform;
            targetCamera.SetActive(true);
            tpsCamera.SetActive(false);
            tpsOff = true;

            if (showEnemyLockedOn)
                Debug.Log(enemyLocked);
        }
    }
    /// <summary>
    /// Gathers information around player in based on <c>detectRadius</c>
    /// </summary>
    /// <returns>Enemy objects nearby</returns>
    private ArrayList GetNearbyEnemies()
	{
        ArrayList enemies = new ArrayList();

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
				enemies.Add(colliders[i].gameObject);
        }

        return enemies;
	}
    /// <summary>
    /// Converts an <c>ArrayList</c> to a <c>GameObject[]</c>
    /// </summary>
    /// <param name="enemies"><c>ArrayList</c> to be converted</param>
    /// <returns></returns>
    private GameObject[] ConvertArrayList(ArrayList enemies)
	{
        GameObject[] arrayEnemies = new GameObject[enemies.Count];

        for (int i = 0; i < enemies.Count; i++)
            arrayEnemies[i] = (GameObject)enemies[i];

        return arrayEnemies;
	}
    /// <summary>
    /// A sorting algorithm that sorts based on distance of enemy and the player
    /// </summary>
    /// <param name="enemies"></param>
    /// <param name="firstIndex"></param>
    /// <param name="lastIndex"></param>
    private void QuickSort(GameObject[] enemies, int firstIndex, int lastIndex)
	{
        if (firstIndex < lastIndex)
		{
            int medianNumber = Partition(enemies, firstIndex, lastIndex);
            QuickSort(enemies, firstIndex, medianNumber - 1);
            QuickSort(enemies, medianNumber + 1, lastIndex);
		}
	}
    /// <summary>
    /// Sorts gameobjects that have less distance than the pivot
    /// </summary>
    /// <param name="enemies"></param>
    /// <param name="firstIndex"></param>
    /// <param name="lastIndex"></param>
    /// <returns></returns>
    private int Partition(GameObject[] enemies, int firstIndex, int lastIndex)
	{
        GameObject enemyPivot = enemies[lastIndex];
        float distancePivot = Vector3.Distance(enemyPivot.transform.position, transform.position);
        int i = firstIndex - 1;
        GameObject tempEnemy;
        GameObject tempTemp;

        for (int k = firstIndex; k <= lastIndex - 1; k++)
		{
            tempEnemy = enemies[k];
            float distanceTemp = Vector3.Distance(tempEnemy.transform.position, transform.position);

            if (distanceTemp <= distancePivot)
			{
                i++;
                tempTemp = enemies[i];
                enemies[i] = tempEnemy;
				enemies[k] = tempTemp;
			}
		}
        tempTemp = enemies[i + 1];
        enemies[i + 1] = enemyPivot;
        enemies[lastIndex] = tempTemp;

        return i + 1;
	}
    /// <summary>
    /// Checks if <paramref name="enemies"/> has the same objects as <c>enemiesInRadius</c>
    /// up until <c>currentEnemy</c>
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns>True if both arrays are the same up until <c>currentEnemy</c>. False otherwise</returns>
    private bool SameEnemiesCheck(GameObject[] enemies)
	{
        if (enemiesInRadius == null)
            return false;

        if (enemies.Length != enemiesInRadius.Length)
            return false;

        for (int i = 0; i <= currentEnemy; i++)
		{
            if (enemies[i].GetInstanceID() != enemiesInRadius[i].GetInstanceID())
                return false;
		}

        return true;
	}
    private void LookAtEnemy()
	{
        Vector3 target = enemyLocked.transform.position - transform.position;
        Quaternion rotat = Quaternion.LookRotation(target.normalized);
        //Locks rotation on x and z axis
        rotat.x = 0;
        rotat.z = 0;
        transform.rotation = rotat;
    }
    /// <summary>
    /// Sets the target camera to the third person camera's position
    /// </summary>
    public void SetTargetTransform()
    {
        targetCamera.transform.position = tpsCamera.transform.position;
    }
    /// <summary>
    /// Sets the third person camera to the target camera's position
    /// </summary>
    public void SetTPSTransform()
    {
        tpsCamera.transform.position = targetCamera.transform.position;
    }
	private void OnDrawGizmos()
	{
        //Draws which enemy the player is currently targeting
		Gizmos.color = Color.yellow;
        if (enemyLocked != null)
            Gizmos.DrawWireSphere(enemyLocked.transform.position, 5);
	}
	private void OnDrawGizmosSelected()
    {
        //Draws the targeting radius the player has
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}