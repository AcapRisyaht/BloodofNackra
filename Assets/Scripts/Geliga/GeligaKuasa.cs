using UnityEngine;

[CreateAssetMenu(fileName = "GeligaData", menuName = "Geliga/Kuasa")]
public class GeligaKuasa : ScriptableObject
{
  public JenisGeliga jenis;
  public string namaGeliga;
  public string keterangan;
  public Sprite ikonGeliga;
  public GameObject efekPrefab;
  public AudioClip bunyiAktif;
}
