using UnityEngine;

public class KeepObjectInPlace : MonoBehaviour
{
    private bool isPlaced = false;

    void Update()
    {
        // تحقق إذا كانت الصورة تم التعرف عليها والطاولة لم يتم فصلها بعد
        // (نعتبر أن الطاولة هي أول ابتلعته أو الكائن الفرعي الأول هنا)
        transform.GetChild(0); 
    }

    // هذه الدالة تربطها بمكون Default Observer Event Handler الموجود على الImage Target
    public void DetachTableFromTarget()
    {
        if (!isPlaced && transform.childCount > 0)
        {
            // فصل الطاولة عن الصورة لتصبح حرة وثابتة في مكانها بالعالم الواقعي
            Transform table = transform.GetChild(0);
            table.SetParent(null); 
            isPlaced = true;
            
            // إيقاف هذا السكريبت بعد إتمام الفصل
            enabled = false;
        }
    }
}