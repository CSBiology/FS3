namespace FS3

module AccessBasic =

    open Amazon.S3
    open Amazon.S3.Model

    let private initPutObjectFromFileAsync (client: AmazonS3Client) (objectPath: string) (bucket: string) =

        let objectKey = System.IO.Path.GetFileName objectPath

        let putRequest = 
            let request = new PutObjectRequest ()
            request.BucketName <- bucket
            request.Key <- objectKey
            request.FilePath <- objectPath
            request

        let request = client.PutObjectAsync(putRequest) |> Async.AwaitTask
        request

    let private initCopyObjectAsync (client: AmazonS3Client) (sourceObjectKey: string) (targetObjectKey: string) (sourceBucket: string) (targetBucket: string) =

        let copyObjectRequest =
            let request = new CopyObjectRequest()
            request.SourceBucket <- sourceBucket
            request.DestinationBucket <- targetBucket
            request.SourceKey <- sourceObjectKey
            request.DestinationKey <- targetObjectKey
            request

        let request = client.CopyObjectAsync(copyObjectRequest) |> Async.AwaitTask
        request

    let private initDeleteObjectAsync (client: AmazonS3Client) (objectKey: string) (bucket: string) =

        let deleteRequest = 
            let request = new DeleteObjectRequest ()
            request.BucketName <- bucket
            request.Key <- objectKey
            request

        let request = client.DeleteObjectAsync(deleteRequest) |> Async.AwaitTask
        request

    let private initReadObjectDataAsync (client: AmazonS3Client) (objectKey: string) (bucket: string) =
    
        let getObjectRequest = 
            let request = new GetObjectRequest()
            request.BucketName <- bucket
            request.Key <- objectKey
            request

        let request = client.GetObjectAsync(getObjectRequest) |> Async.AwaitTask
        request

    let uploadObjectFromFile client  sourceFilePath targetFolderPath =

        initPutObjectFromFileAsync client sourceFilePath targetFolderPath
        |> Async.RunSynchronously
        |> ignore

    let copyObject client sourceObjectKey targetObjectKey sourceBucket targetBucket =

        initCopyObjectAsync client sourceObjectKey targetObjectKey sourceBucket targetBucket
        |> Async.RunSynchronously
        |> ignore

    let deleteObject client objectKey bucket =
    
        initDeleteObjectAsync client objectKey bucket
        |> Async.RunSynchronously
        |> ignore

    let downloadObject client objectKey bucket targetPath =
    
        use response =
            initReadObjectDataAsync client objectKey bucket
            |> Async.RunSynchronously
        use responseStream =
            response.ResponseStream

        responseStream.CopyTo (new FileStream(targetPath, FileMode.Create))