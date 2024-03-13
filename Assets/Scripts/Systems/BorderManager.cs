using UnityEngine;

public class BorderManager : MonoBehaviour
{
    [SerializeField] private GameObject borderPrefab;
    public Border[] borders;

    public void GenerateBorder(FieldType[,] platform, int PLATFORM_SIZE) {
        for(int i = 0;i < PLATFORM_SIZE;i++) {
            for(int j = 0;j < PLATFORM_SIZE;j++) {
                if(platform[i, j] == FieldType.PLATFORM) {
                    FindBorderAroundPlatform(i, j);
                }
            }
        }

        void FindBorderAroundPlatform(int x, int y) {
            CheckPosition(x - 1, y - 1);
            CheckPosition(x, y - 1);
            CheckPosition(x + 1, y - 1);

            CheckPosition(x - 1, y);
            CheckPosition(x + 1, y);

            CheckPosition(x - 1, y + 1);
            CheckPosition(x, y + 1);
            CheckPosition(x + 1, y + 1);
        }

        void CheckPosition(int x, int y) {
            if(platform[x, y] != FieldType.NULL) {
                return;
            }

            //Create an array
            byte[] values = new byte[9];

            if(x - 1 < 0) {
                values[0] = 0;
                values[3] = 0;
                values[6] = 0;
            } else {
                if(y - 1 < 0) {
                    values[0] = 0;
                } else {
                    values[0] = (byte)(platform[x - 1, y - 1] == FieldType.PLATFORM ? 1 : 0);
                }

                values[3] = (byte)(platform[x - 1, y] == FieldType.PLATFORM ? 1 : 0);

                if(y + 1 >= PLATFORM_SIZE) {
                    values[6] = 0;
                } else {
                    values[6] = (byte)(platform[x - 1, y + 1] == FieldType.PLATFORM ? 1 : 0);
                }
            }

            if(x + 1 >= PLATFORM_SIZE) {
                values[2] = 0;
                values[5] = 0;
                values[8] = 0;
            } else {
                if(y - 1 < 0) {
                    values[2] = 0;
                } else {
                    values[2] = (byte)(platform[x + 1, y - 1] == FieldType.PLATFORM ? 1 : 0);
                }

                values[5] = (byte)(platform[x + 1, y] == FieldType.PLATFORM ? 1 : 0);

                if(y + 1 >= PLATFORM_SIZE) {
                    values[8] = 0;
                } else {
                    values[8] = (byte)(platform[x + 1, y + 1] == FieldType.PLATFORM ? 1 : 0);
                }
            }

            if(y - 1 < 0) {
                values[1] = 0;
            } else {
                values[1] = (byte)(platform[x, y - 1] == FieldType.PLATFORM ? 1 : 0);
            }

            values[4] = (byte)(platform[x, y] == FieldType.PLATFORM ? 1 : 0);

            if(y + 1 >= PLATFORM_SIZE) {
                values[7] = 0;
            } else {
                values[7] = (byte)(platform[x, y + 1] == FieldType.PLATFORM ? 1 : 0);
            }


            foreach(Border border in borders) {
                if(border.Compare(values)) {
                    GameObject borderObj = Instantiate(borderPrefab);
                    borderObj.GetComponent<SpriteRenderer>().sprite = border.sprite;
                    borderObj.transform.position = new Vector3(x, -y, 1);
                    borderObj.transform.SetParent(transform, true);
                }
            }

            platform[x, y] = FieldType.BORDER;
        }
    }

    [System.Serializable]
    public struct Border {
        public Sprite sprite;
        public byte[] border;

        public bool Compare(byte[] values) {
            for(int i = 0;i < 9;i++) {
                if(border[i] != values[i] && border[i]!=2) {
                    return false;
                }
            }
            return true;
        }
    }
}
