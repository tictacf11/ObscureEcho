using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridLayoutGroup : GridLayoutGroup
{
    [SerializeField] int m_Columns;
    [SerializeField] int m_Rows;
    [SerializeField] float m_CellAspectRatio;

    protected override void Awake()
    {
        UpdateCellSizeByRowsAndColumns(m_Rows, m_Columns);
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        UpdateCellSizeByRowsAndColumns(m_Rows, m_Columns);
    }

    public void UpdateCellSizeByRowsAndColumns(int rows, int columns)
    {
        constraint = Constraint.FixedColumnCount;
        constraintCount = columns;

        if (rows <= 0) rows = 1;
        if(columns <= 0) columns = 1;
        if(m_CellAspectRatio <= 0) m_CellAspectRatio = 1;

        float horizontalPadding = padding.left + padding.right;
        float verticalPadding = padding.top + padding.bottom;
        float horizontalSpacing = spacing.x * (columns - 1);
        float verticalSpacing = spacing.y * (rows - 1);
        float width = rectTransform.rect.width - horizontalPadding - horizontalSpacing;
        float height = rectTransform.rect.height - verticalPadding - verticalSpacing;

        float cellWidth = width / columns;
        float cellHeight = height / rows;

        float aspectRatioHeight = cellWidth / m_CellAspectRatio;
        float aspectRatioWidth = cellHeight * m_CellAspectRatio;

        if (aspectRatioHeight <= cellHeight)
        {
            cellHeight = aspectRatioHeight;
        }
        else
        {
            cellWidth = aspectRatioWidth;
        }

        cellSize = new Vector2(cellWidth, cellHeight);

        m_Columns = columns;
        m_Rows = rows;

        SetDirty();
    }
}
