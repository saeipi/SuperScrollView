﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView
{

    public class ItemData
    {
        public int mId;
        public string mName;
        public int mFileSize;
        public string mDesc;
        public string mIcon;
        public int mStarCount;
        public bool mChecked;
        public bool mIsExpand;
    }

    public class DataSourceMgr : MonoBehaviour
    {
        //列表数据
        List<ItemData> mItemDataList = new List<ItemData>();
        //刷新完成回调
        System.Action mOnRefreshFinished = null;
        //加载更多数据完成
        System.Action mOnLoadMoreFinished = null;
        //加载更多数据数量
        int mLoadMoreCount = 20;
        //数据加载时间
        float mDataLoadLeftTime = 0;
        //是否等待刷新数据
        bool mIsWaittingRefreshData = false;
        //是否等待加载更多数据
        bool mIsWaitLoadingMoreData = false;
        //数据总条数
        public int mTotalDataCount = 10000;
        //数据管理对象
        static DataSourceMgr instance = null;

        public static DataSourceMgr Get
        {
            get
            {
                if (instance == null)
                {
                    instance = Object.FindObjectOfType<DataSourceMgr>();
                }
                return instance;
            }

        }

        void Awake()
        {
            Init();
        }


        public void Init()
        {
            DoRefreshDataSource();
        }

        //获取index的数据模型
        public ItemData GetItemDataByIndex(int index)
        {
            if (index < 0 || index >= mItemDataList.Count)
            {
                return null;
            }
            return mItemDataList[index];
        }
        //获取指定id的数据模型
        public ItemData GetItemDataById(int itemId)
        {
            int count = mItemDataList.Count;
            for (int i = 0; i < count; ++i)
            {
                if(mItemDataList[i].mId == itemId)
                {
                    return mItemDataList[i];
                }
            }
            return null;
        }
        
        //获取数据条数
        public int TotalItemCount
        {
            get
            {
                return mItemDataList.Count;
            }
        }
        /// <summary>
        /// 加载最新数据
        /// </summary>
        /// <param name="onReflushFinished"></param>
        public void RequestRefreshDataList(System.Action onReflushFinished)
        {
            mDataLoadLeftTime = 1;
            mOnRefreshFinished = onReflushFinished;//回调
            mIsWaittingRefreshData = true;
        }
        /// <summary>
        /// 加载更多数据
        /// </summary>
        /// <param name="loadCount"></param>
        /// <param name="onLoadMoreFinished"></param>
        public void RequestLoadMoreDataList(int loadCount,System.Action onLoadMoreFinished)
        {
            mLoadMoreCount = loadCount;
            mDataLoadLeftTime = 1;
            mOnLoadMoreFinished = onLoadMoreFinished;//回调
            mIsWaitLoadingMoreData = true;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        public void Update()
        {
            if (mIsWaittingRefreshData)
            {
                mDataLoadLeftTime -= Time.deltaTime;
                if (mDataLoadLeftTime <= 0)
                {
                    mIsWaittingRefreshData = false;
                    DoRefreshDataSource();
                    if (mOnRefreshFinished != null)
                    {
                        mOnRefreshFinished();
                    }
                }
            }
            else if (mIsWaitLoadingMoreData)
            {//如果是加载更多数据
                mDataLoadLeftTime -= Time.deltaTime;
                if (mDataLoadLeftTime <= 0)
                {
                    mIsWaitLoadingMoreData = false;
                    //请求数据
                    DoLoadMoreDataSource();
                    if (mOnLoadMoreFinished != null)
                    {
                        //加载数据完成回调
                        mOnLoadMoreFinished();
                    }
                }
            }

        }
        /// <summary>
        /// 更新数据源数量
        /// </summary>
        /// <param name="count"></param>
        public void SetDataTotalCount(int count)
        {
            mTotalDataCount = count;
            DoRefreshDataSource();
        }
        /// <summary>
        /// 开始重新更新数据源
        /// </summary>
        void DoRefreshDataSource()
        {
            mItemDataList.Clear();
            for (int i = 0; i < mTotalDataCount; ++i)
            {
                ItemData tData = new ItemData();
                tData.mId = i;
                tData.mName = "Item" + i;
                tData.mDesc = "Item Desc For Item " + i;
                tData.mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 24));
                tData.mStarCount = Random.Range(0, 6);
                tData.mFileSize = Random.Range(20, 999);
                tData.mChecked = false;
                tData.mIsExpand = false;
                mItemDataList.Add(tData);
            }
        }
        /// <summary>
        /// 加载更多数据
        /// </summary>
        void DoLoadMoreDataSource()
        {
            int count = mItemDataList.Count;
            for (int k = 0; k < mLoadMoreCount; ++k)
            {
                int i = k + count;
                ItemData tData = new ItemData();
                tData.mId = i;
                tData.mName = "Item" + i;
                tData.mDesc = "Item Desc For Item " + i;
                tData.mIcon = ResManager.Get.GetSpriteNameByIndex(Random.Range(0, 24));
                tData.mStarCount = Random.Range(0, 6);
                tData.mFileSize = Random.Range(20, 999);
                tData.mChecked = false;
                tData.mIsExpand = false;
                mItemDataList.Add(tData);
            }
        }
        /// <summary>
        /// 选中所有item
        /// </summary>
        public void CheckAllItem()
        {
            int count = mItemDataList.Count;
            for (int i = 0; i < count; ++i)
            {
                mItemDataList[i].mChecked = true;
            }
        }
        /// <summary>
        /// 不选所有item
        /// </summary>
        public void UnCheckAllItem()
        {
            int count = mItemDataList.Count;
            for (int i = 0; i < count; ++i)
            {
                mItemDataList[i].mChecked = false;
            }
        }
        /// <summary>
        /// 删除所有选中的item
        /// </summary>
        /// <returns></returns>
        public bool DeleteAllCheckedItem()
        {
            int oldCount = mItemDataList.Count;
            mItemDataList.RemoveAll(it => it.mChecked);
            return (oldCount != mItemDataList.Count);
        }

    }

}