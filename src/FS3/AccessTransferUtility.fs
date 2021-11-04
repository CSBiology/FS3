namespace FS3

module AccessTransferUtility =
    
    open Amazon.S3
    open Amazon.S3.Transfer

    let private initObjectMultipartUploadAsync (client: AmazonS3Client) (objectPath: string) (bucket: string) =

        let fileTransferUtility = new TransferUtility(client)
    
        let multipartUploadRequest =
            fileTransferUtility.UploadAsync(objectPath, bucket)
            |> Async.AwaitTask
        multipartUploadRequest

    let private initObjectMultipartDownloadAsync (client: AmazonS3Client) (objectKey: string) (bucket: string) (targetPath: string)=

        let fileTransferUtility = new TransferUtility(client)
    
        let multipartUploadRequest =
            fileTransferUtility.DownloadAsync(targetPath, bucket, objectKey)
            |> Async.AwaitTask
        multipartUploadRequest

    let private initDirectoryMultipartUploadAsync (client: AmazonS3Client) (directoryPath: string) (bucket: string) (searchPattern: string) =

        let directoryTransferUtility = new TransferUtility(client)
    
        let multipartUploadRequest =
            directoryTransferUtility.UploadDirectoryAsync(directoryPath, bucket, searchPattern, System.IO.SearchOption.AllDirectories)
            |> Async.AwaitTask
        multipartUploadRequest

    let private initDirectoryMultipartDownloadAsync (client: AmazonS3Client) (sourceDirectoryPath: string) (targetDirectoryPath: string) (bucket: string) =

        let directoryTransferUtility = new TransferUtility(client)
    
        let multipartDownloadRequest =
            directoryTransferUtility.DownloadDirectoryAsync(bucket, sourceDirectoryPath, targetDirectoryPath)
            |> Async.AwaitTask
        multipartDownloadRequest

    /// Uploads an object to the S3 storage using multipart upload.
    /// objectPath: Path of the object you want to upload
    /// bucket: Destination of the file on the S3 storage. Subfolders of a bucket have to be specified here as well
    /// e.g. bucketName/folder1/folder2
    let objectMultipartUpload (client: AmazonS3Client) (objectPath: string) (bucket: string) =

        initObjectMultipartUploadAsync client objectPath bucket
        |> Async.RunSynchronously
        |> ignore

    /// Downloads an object from the S3 storage using multipart download.
    /// objectKey: Name of the object you want to download
    /// bucket: Destination of the file on the S3 storage. Subfolders of a bucket have to be specified here as well
    /// e.g. bucketName/folder1/folder2
    /// targetPath: Destiantion path for the object on your PC
    let objectMultipartDownload (client: AmazonS3Client) (objectKey: string) (bucket: string) (targetPath: string)=

        initObjectMultipartDownloadAsync client objectKey bucket targetPath
        |> Async.RunSynchronously
        |> ignore

    /// Recursively uploads a directory to the S3 storage using multipart upload.
    /// directoryPath: Path of the directory you want to upload
    /// bucket: Destination of the directory on the S3 storage. Subfolders of a bucket have to be specified here as well
    /// e.g. bucketName/folder1/folder2
    let directoryMultipartUpload (client: AmazonS3Client) (directoryPath: string) (bucket: string) =

        initDirectoryMultipartUploadAsync client directoryPath bucket "*"
        |> Async.RunSynchronously
        |> ignore
    
    /// Selectively and recursively uploads files from a directory matching the selectPattern to the S3 storage using multipart upload.
    /// directoryPath: Path of the directory you want to upload
    /// bucket: Destination of the directory on the S3 storage. Subfolders of a bucket have to be specified here as well
    /// e.g. bucketName/folder1/folder2
    /// selectPattern: Search pattern for files (e.g. "*.txt" for txt files)
    let directoryMultipartUploadSelective (client: AmazonS3Client) (directoryPath: string) (bucket: string) (selectPattern: string) =

        initDirectoryMultipartUploadAsync client directoryPath bucket selectPattern
        |> Async.RunSynchronously
        |> ignore

    /// Downloads a directory from the S3 storage using multipart download.
    /// bucket: Name of the bucket on the S3 storage
    /// sourceDirectoryPath: Path of the directory on the s3 storage
    /// targetDirectoryPath: Destiantion path for the directory on your PC
    let directoryMultipartDownload (client: AmazonS3Client) (sourceDirectoryPath: string) (targetDirectoryPath: string) (bucket: string) =

        initDirectoryMultipartDownloadAsync client sourceDirectoryPath targetDirectoryPath bucket
        |> Async.RunSynchronously
        |> ignore