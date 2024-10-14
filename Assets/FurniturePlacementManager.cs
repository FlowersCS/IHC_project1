using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FurniturePlacementManager : MonoBehaviour
{
    public GameObject spawnableFurniture;
    public ARRaycastManager raycastManager;
    public PlacementIndicator placementIndicator;
    public AudioSource audioSource;

    private List<GameObject> placedFurniture = new List<GameObject>();

    private int scaleOption = 0; // 0: original, 1: 1.2x, 2: 0.4x

    public void SwitchFurniture(GameObject furniture)
    {
        spawnableFurniture = furniture;
        scaleOption = 0;
        StartCoroutine(PlaceFurniture());
    }

    private IEnumerator PlaceFurniture()
    {
        if (spawnableFurniture != null)
        {
            GameObject spawnedObject = Instantiate(spawnableFurniture, placementIndicator.transform.position, placementIndicator.transform.rotation);
            placedFurniture.Add(spawnedObject);
            audioSource.Play();
            yield return StartCoroutine(AnimateCreation(spawnedObject));
        }
    }

    private IEnumerator AnimateCreation(GameObject obj)
    {
        Vector3 targetScale = Vector3.one; // Escala final
        Vector3 initialScale = Vector3.zero; // Escala inicial (invisible)
        float duration = 0.5f; // Duración de la animación
        float time = 0;

        obj.transform.localScale = initialScale; // Comienza en escala 0

        while (time < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration); // Interpola la escala
            time += Time.deltaTime;
            yield return null; // Espera el siguiente frame
        }
        obj.transform.localScale = targetScale; // Asegúrate de que llegue a la escala final
    }

    public void RemoveLastFurniture()
    {
        if (placedFurniture.Count > 0)
        {
            GameObject lastFurniture = placedFurniture[placedFurniture.Count - 1];
            audioSource.Play();
            StartCoroutine(AnimateRemoval(lastFurniture)); // Llama a la animación suave
        }
    }

    private IEnumerator AnimateRemoval(GameObject obj)
    {
        Vector3 targetScale = Vector3.zero; // Escala final (invisible)
        float duration = 0.5f; // Duración de la animación
        float time = 0;

        Vector3 initialScale = obj.transform.localScale; // Escala inicial

        while (time < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration); // Interpola la escala
            time += Time.deltaTime;
            yield return null; // Espera el siguiente frame
        }

        Destroy(obj); // Destruye el objeto después de la animación
    }

    public void RotateLastFurniture()
    {
        if (placedFurniture.Count > 0)
        {
            GameObject lastFurniture = placedFurniture[placedFurniture.Count - 1];
            audioSource.Play();
            StartCoroutine(AnimateRotation(lastFurniture)); // Llama a la animación suave
        }
    }

    private IEnumerator AnimateRotation(GameObject obj)
    {
        Quaternion initialRotation = obj.transform.rotation; // Rotación inicial
        Quaternion targetRotation = Quaternion.Euler(obj.transform.eulerAngles + new Vector3(0, 45, 0)); // Rotación final
        float duration = 0.5f; // Duración de la animación
        float time = 0;

        while (time < duration)
        {
            obj.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, time / duration); // Interpola la rotación
            time += Time.deltaTime;
            yield return null; // Espera el siguiente frame
        }

        obj.transform.rotation = targetRotation; // Asegúrate de que llegue a la rotación final
    }

    public void ScaleUp()
    {
        if (placedFurniture.Count > 0)
        {
            GameObject lastFurniture = placedFurniture[placedFurniture.Count - 1];
            StartCoroutine(AnimateScale(lastFurniture, 1.2f)); // Llama a la animación suave
        }
    }

    public void ScaleDown()
    {
        if (placedFurniture.Count > 0)
        {
            GameObject lastFurniture = placedFurniture[placedFurniture.Count - 1];
            StartCoroutine(AnimateScale(lastFurniture, 0.4f)); // Llama a la animación suave}
        }
    }

    private IEnumerator AnimateScale(GameObject obj, float targetScaleFactor)
    {
        Vector3 initialScale = obj.transform.localScale; // Escala inicial
        Vector3 targetScale = initialScale * targetScaleFactor; // Escala final
        float duration = 0.5f; // Duración de la animación
        float time = 0;

        while (time < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration); // Interpola la escala
            time += Time.deltaTime;
            yield return null; // Espera el siguiente frame
        }

        obj.transform.localScale = targetScale; // Asegúrate de que llegue a la escala final
    }

    public void ResetScale()
    {
        if (placedFurniture.Count > 0)
        {
            GameObject lastFurniture = placedFurniture[placedFurniture.Count - 1];
            StartCoroutine(AnimateResetScale(lastFurniture)); // Llama a la animación suave
        }
    }

    private IEnumerator AnimateResetScale(GameObject obj)
    {
        Vector3 initialScale = obj.transform.localScale; // Escala inicial
        Vector3 targetScale = Vector3.one; // Escala final
        float duration = 0.5f; // Duración de la animación
        float time = 0;

        while (time < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration); // Interpola la escala
            time += Time.deltaTime;
            yield return null; // Espera el siguiente frame
        }

        obj.transform.localScale = targetScale; // Asegúrate de que llegue a la escala final
    }

    public void ScaleLastFurniture()
    {
        if (placedFurniture.Count > 0)
        {
            if (scaleOption == 0)
            {
                scaleOption = 1;
                audioSource.Play();
                ScaleUp();
            }
            else if (scaleOption == 1)
            {
                scaleOption = 2;
                audioSource.Play();
                ScaleDown();
            }
            else if (scaleOption == 2)
            {
                scaleOption = 0;
                audioSource.Play();
                ResetScale();
            }
        }
    }
}
