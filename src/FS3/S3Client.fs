namespace FS3

module S3Client =
    
    open Amazon.S3
    
    let initS3UniKl (accessKey: string) (secretAccessKey: string) =
        let config = new AmazonS3Config()
        config.ServiceURL <- "https://s3.uni-kl.de/"
        config.UseDualstackEndpoint <- true
        config.ForcePathStyle <- true
        let client = new AmazonS3Client (accessKey,secretAccessKey,config)
        client
