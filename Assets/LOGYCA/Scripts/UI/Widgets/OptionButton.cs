using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LOGYCA.OSA.Data;

namespace LOGYCA.OSA.UI.Widgets
{
    /// <summary>
    /// Botón de opción del frame 03. Estados visuales:
    ///   - Normal:   btn_opcion_normal.png
    ///   - Hover:    btn_opcion_hover_naranja.png
    ///   - Activo:   btn_opcion_activo_teal.png
    /// </summary>
    [RequireComponent(typeof(Image), typeof(Button))]
    public class OptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text textoLetra;
        [SerializeField] private TMP_Text textoOpcion;
        [SerializeField] private Image fondo;
        [SerializeField] private Button button;

        [Header("Sprites estados")]
        [SerializeField] private Sprite spriteNormal;
        [SerializeField] private Sprite spriteHover;
        [SerializeField] private Sprite spriteActivo;

        private Action onClick;
        private bool seleccionado;

        public void Configurar(OpcionData opcion, Action onClick)
        {
            this.onClick = onClick;
            seleccionado = false;
            if (textoLetra != null) textoLetra.text = opcion.letra.ToString();
            if (textoOpcion != null) textoOpcion.text = opcion.titulo;
            SetSprite(spriteNormal);

            if (button == null) button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                seleccionado = true;
                SetSprite(spriteActivo);
                this.onClick?.Invoke();
            });
        }

        public void OnPointerEnter(PointerEventData _)
        {
            if (!seleccionado) SetSprite(spriteHover);
        }

        public void OnPointerExit(PointerEventData _)
        {
            if (!seleccionado) SetSprite(spriteNormal);
        }

        private void SetSprite(Sprite s)
        {
            if (fondo != null && s != null) fondo.sprite = s;
        }
    }
}
