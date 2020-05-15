﻿using UnityEngine;
using Verse;

namespace InGameWiki
{
    public class WikiElement
    {
        public static WikiElement Create(string text)
        {
            return Create(text, null, null);
        }

        public static WikiElement Create(Texture2D image, Vector2? imageSize = null)
        {
            return Create(null, image, imageSize);
        }

        public static WikiElement Create(string text, Texture2D image, Vector2? imageSize = null)
        {
            return new WikiElement()
            {
                Text = text,
                Image = image,
                ImageSize = imageSize ?? new Vector2(-1, -1)
            };
        }

        public string Text;
        public Texture2D Image;
        public bool AutoFitImage = false;
        public Vector2 ImageSize = new Vector2(-1, -1);
        public float ImageScale = 1f;
        public GameFont FontSize = GameFont.Small;
        public Def DefForIconAndLabel;

        public bool HasText
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Text);
            }
        }
        public bool HasImage
        {
            get
            {
                return Image != null;
            }
        }

        public virtual Vector2 Draw(Rect maxBounds)
        {
            Vector2 size = Vector2.zero;

            var old = Verse.Text.Font;
            Verse.Text.Font = FontSize;

            Vector2 imageOffset = Vector2.zero;
            if (HasImage)
            {
                if (!AutoFitImage)
                {
                    float width = ImageSize.x < 1 ? Image.width * ImageScale : ImageSize.x;
                    float height = ImageSize.y < 1 ? Image.height * ImageScale : ImageSize.y;
                    Widgets.DrawTextureFitted(new Rect(maxBounds.x, maxBounds.y, width, height), Image, 1f);
                    size += new Vector2(width, height);
                    imageOffset.x = width;
                }
                else
                {
                    float baseWith = Image.width;

                    if(baseWith <= maxBounds.width)
                    {
                        float width = Image.width;
                        float height = Image.height;
                        Widgets.DrawTextureFitted(new Rect(maxBounds.x, maxBounds.y, width, height), Image, 1f);
                        size += new Vector2(width, height);
                        imageOffset.x = width;
                    }
                    else
                    {
                        float width = maxBounds.width;
                        float height = Image.height * (width / Image.width);
                        Widgets.DrawTextureFitted(new Rect(maxBounds.x, maxBounds.y, width, height), Image, 1f);
                        size += new Vector2(width, height);
                        imageOffset.x = width;
                    }
                }
            }

            if (DefForIconAndLabel != null)
            {
                //TooltipHandler.TipRegion(rect, (TipSignal)def.description);
                var rect = new Rect(maxBounds.x, maxBounds.y, 200, 32);
                Widgets.DefLabelWithIcon(rect, DefForIconAndLabel);
                if (Widgets.ButtonInvisible(rect, true))
                {
                    WikiWindow.CurrentActive?.GoToPage(this.DefForIconAndLabel, true);
                }
                size.y += 32;
                imageOffset.x = 200;
                imageOffset.y = 6;
            }

            if (HasText)
            {
                float x = maxBounds.x + imageOffset.x;
                float width = maxBounds.xMax - x;

                float startY = maxBounds.y + imageOffset.y;
                float cacheStartY = startY;
                Widgets.LongLabel(x, width, Text, ref startY);
                float change = startY - cacheStartY;

                size += new Vector2(width, 0);
                if (size.y < change)
                    size.y = change;
            }

            Verse.Text.Font = old;

            return size;
        }
    }
}
