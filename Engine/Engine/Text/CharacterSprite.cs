namespace Engine
{
  public class CharacterSprite
  {
    public Sprite Sprite { get; set; }
    public CharacterData Data { get; set; }

    public CharacterSprite(Sprite sprite, CharacterData data)
    {
      Data = data;
      Sprite = sprite;
    }
  }
}