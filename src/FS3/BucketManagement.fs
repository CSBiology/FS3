namespace FS3


module BucketManagement =

    open Amazon.S3
    open Amazon.S3.Model
    open System.IO

    let private initListBuckets (client: AmazonS3Client) =
        client.ListBucketsAsync()
        |> Async.AwaitTask

    let private initCreateBucket (client: AmazonS3Client) (bucketName: string)=
        client.PutBucketAsync(bucketName)
        |> Async.AwaitTask

    let private initDeleteBucket (client: AmazonS3Client) (bucketName: string)=
        client.DeleteBucketAsync(bucketName)
        |> Async.AwaitTask

    let listBuckets (client: AmazonS3Client) =
        initListBuckets client
        |> Async.RunSynchronously
        |> ignore

    let createBucket (client: AmazonS3Client) (bucketName: string) =
        initCreateBucket client bucketName
        |> Async.RunSynchronously
        |> ignore

    let deleteBucket (client: AmazonS3Client) (bucketName: string) =
        initDeleteBucket client bucketName
        |> Async.RunSynchronously
        |> ignore