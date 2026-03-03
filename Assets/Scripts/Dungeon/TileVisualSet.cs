using UnityEngine;

[System.Serializable]
public class TileVisualSet
{
    public TileType type; // ¿Qué es? (Wall, Building, Park...)

    [Header("Suelo")]
    public Sprite[] floor;
    public Sprite[] leftFloor;   
    public Sprite[] rightFloor;  

    [Header("Techo / Pasillo")]
    public Sprite[] corridor; 

    [Header("Paredes Frontales")]
    public Sprite[] frontWall; 

    [Header("Paredes Laterales (Inmediatas)")]
    public Sprite[] leftWall;    
    public Sprite[] rightWall;   

    [Header("Paredes Diagonales (Al fondo)")]
    public Sprite[] frontLeftWall;  
    public Sprite[] frontRightWall; 
}