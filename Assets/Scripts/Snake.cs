using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private int _cycles = 10;
    private int _cyclesPassed = 0;
    private List<Transform> _segments;
    private bool _ableToDie = false;

    public int speed;
    public int segments;
    public Transform segmentPrefab;

    private void Start()
    {
      _segments = new List<Transform> { this.transform };
      
      gameObject.transform.eulerAngles = new Vector3(
        0,
        0,
        -90
      );

      for(int i = 0; i < this.segments; i++)
      {
        Grow();
      }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && (_direction != Vector2.down || _segments.Count == 1)) {
          _direction = Vector2.up;
          gameObject.transform.eulerAngles = new Vector3(
            0,
            0,
            0
          );
        } else if(Input.GetKeyDown(KeyCode.S) && (_direction != Vector2.up || _segments.Count == 1)) {
          _direction = Vector2.down;
          gameObject.transform.eulerAngles = new Vector3(
            0,
            0,
            180
          );
        } else if(Input.GetKeyDown(KeyCode.D) && (_direction != Vector2.left || _segments.Count == 1)) {
          _direction = Vector2.right;
          gameObject.transform.eulerAngles = new Vector3(
            0,
            0,
            -90
          );
        } else if(Input.GetKeyDown(KeyCode.A) && (_direction != Vector2.right || _segments.Count == 1)) {
          _direction = Vector2.left;
          gameObject.transform.eulerAngles = new Vector3(
            0,
            0,
            90
          );
        }
    }

    private void FixedUpdate()
    {
      if(_cycles <= 0) {
        for(int i = _segments.Count - 1; i > 0; i--)
        {
          _segments[i].position = _segments[i - 1].position;
        }

        transform.position = new Vector3(
          Mathf.Round(transform.position.x) + _direction.x,
          Mathf.Round(transform.position.y) + _direction.y,
          0.0f
        );
        _cycles = 10;

        if(_cyclesPassed > segments + 1) {
          _ableToDie = true;
        } else {
          _cyclesPassed++;
        }
      }
      _cycles = (_cycles - speed);
    }

    private void ResetGame()
    {
      for (int i = 1; i < _segments.Count; i++)
      {
        Destroy(_segments[i].gameObject);
      }
      _cyclesPassed = 0;
      _ableToDie = false;
      _segments.Clear();
      transform.position = new Vector3(0.0f, 0.0f, 0.0f);
      Start();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Food")) {
          Grow();
          _ableToDie = false;
        } else if(other.CompareTag("Obstacle") && _ableToDie) {
          ResetGame();
        }
    }

    private void Grow()
    {
      Transform segment = Instantiate(this.segmentPrefab);
      segment.position = _segments[_segments.Count - 1].position;

      _segments.Add(segment);
    }
}
