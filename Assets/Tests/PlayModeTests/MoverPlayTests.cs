using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RPG.Movement;
using UnityEngine;
using UnityEngine.TestTools;

public class MoverPlayTests
{
    /*
    PlayerMover mover;
    SerializedObject so;
    Transform cameraFocus;

    [SetUp]
    public void SetUp()
    {
        mover = new GameObject().AddComponent<CharacterController>().gameObject.AddComponent<PlayerMover>();
        so = new SerializedObject(mover);
        cameraFocus = new GameObject().GetComponent<Transform>();
        so.FindProperty("cameraFocus").objectReferenceValue = cameraFocus;
        so.ApplyModifiedProperties();
    }

    static float[] xValues = new float[] { 0, 1, -1, 20, 0.00001f };
    static float[] yValues = new float[] { 0, 3, -3, -20, -0.0000001f };
    [UnityTest]
    public IEnumerator MoverMovesCorrectly([ValueSource("xValues")] float x, [ValueSource("yValues")] float z)
    {
        Vector3 direction = new Vector3(x, 0, z);
        mover.currentMovement = direction;
        Vector3 oldPosition = mover.transform.position;
        float deltaTime = Time.deltaTime;
        mover.Move();
        yield return null;
        float movementSpeed = so.FindProperty("movementSpeed").floatValue;
        Assert.AreEqual(oldPosition.x + movementSpeed * deltaTime * direction.x, mover.transform.position.x, 0.001f);
        Assert.AreEqual(oldPosition.z + movementSpeed * deltaTime * direction.z, mover.transform.position.z, 0.001f);
    }

    [UnityTest]
    public IEnumerator MoverJumpsCorrectly()
    {
        Vector3 direction = new Vector3(0, 1, 0);
        mover.currentMovement = direction;
        Vector3 oldPosition = mover.transform.position;
        yield return null;
        float jumpForce = so.FindProperty("jumpForce").floatValue;
        Assert.AreEqual(oldPosition.y + cameraFocus.up.y * direction.y * jumpForce * Time.deltaTime, mover.transform.position.y, 0.001f);
    }
    */
}
