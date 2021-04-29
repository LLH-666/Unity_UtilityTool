# Unity_UtilityTool
总结一些好用的工具

## 【unity】性能优化之——视锥体剔除(Frustum Culling)
### 问题要点
这里基于Unity提供的原生API来探讨基于视锥体的剔除流程，需要使用到GeometryUtility中提供的API。

1.获取相机的剪裁平面：

有多个API可获得剪裁平面：

①public static Plane[] CalculateFrustumPlanes(Camera camera);

② public static Plane[] CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix)；

③ public static void CalculateFrustumPlanes(Camera camera, Plane[] planes)；

④  public static void CalculateFrustumPlanes(Matrix4x4 worldToProjectionMatrix, Plane[] planes)；

前三个API最终都是调用了④来实现剪裁面获取功能的，其中①和②由于在内部创建了Plane数组，并返回，因此存在GC，而③和④需要预先定义一个长度为6的Plane数组，并传入方法，方法内部会修改这些对象的值，因而不存在GC。所以建议使用③或者④。

通过上述API获取的剪裁平面的顺序依次是：左、右、下、上、近、远。

2.传入需要检测对象的BondingBox：

public static bool TestPlanesAABB(Plane[] planes, Bounds bounds)；

调用上述API，传入通过①获取的剪裁平面及对象的BoundingBox即可检测出该对象是否在视锥体内。

### 存在的问题
通过上述API获取剪裁面时，只能一次性获所有的剪裁面，而在一些特殊情况下我们往往只需要部分剪裁面即可。同时上述API底层采用了P/Invoke方式调用了非托管C++库来实现剪裁面的计算，频繁调用会有一定的性能损耗。为了实现更加个性化的基于视锥体的裁剪方案，我们往往需要自行计算剪裁面，并进行包含检测。下一篇博客将进行详细介绍。

