using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platform;
    public Transform generationPoint;

    public float distanceBetween;
    public float distanceMin;
    public float distanceMax;

    public float distanceIncreaseAmount = 1f; // Значение, на которое будем увеличивать дистанции
    public float distanceIncreaseInterval = 30f; // Интервал времени для увеличения дистанций в секундах

    private float timeSinceLastIncrease; // Время, прошедшее с последнего увеличения дистанций

    public PlatformManager[] platformsM;
    float platwormWidth;

    int platformSelector;
    float[] platformsWidth;

    float minHeight;
    public Transform maxHeightPoint;
    float maxHeight;
    public float maxHeightChange;
    float heightChange;

    public GameObject coinPrefab;
    [Range(0, 1)]
    public float coinSpawnChance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        platformsWidth = new float[platformsM.Length];

        for (int i = 0; i < platformsM.Length; i++)
        {
            platformsWidth[i] = platformsM[i].platform.GetComponent<BoxCollider2D>().size.x;
        }

        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;
        
        timeSinceLastIncrease = 0f; // Инициализация таймера
    }

    // Update is called once per frame
    void Update()
    {
        // Обновляем таймер
        timeSinceLastIncrease += Time.deltaTime;

        // Увеличиваем минимальное и максимальное расстояние, если прошел интервал времени
        if (timeSinceLastIncrease >= distanceIncreaseInterval)
        {
            distanceMin += distanceIncreaseAmount;
            distanceMax += distanceIncreaseAmount;
            timeSinceLastIncrease = 0f; // Сбрасываем таймер
        }

        if (transform.position.x < generationPoint.position.x)
        {
            distanceBetween = Random.Range(distanceMin, distanceMax);

            platformSelector = Random.Range(0, platformsM.Length);

            heightChange = transform.position.y + Random.Range(maxHeightChange, -maxHeightChange);

            if (heightChange > maxHeight)
            {
                heightChange = maxHeight;
            }
            else if (heightChange < minHeight)
            {
                heightChange = minHeight;
            }

            transform.position = new Vector3(transform.position.x + 3 * platformsWidth[platformSelector] + distanceBetween, heightChange, transform.position.z);

            GameObject newPlatform = platformsM[platformSelector].GetPlatform();

            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);

            if (Random.value < coinSpawnChance)
            {
                Vector3 coinPosition = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                GameObject newCoin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);
            }
        }
    }
}