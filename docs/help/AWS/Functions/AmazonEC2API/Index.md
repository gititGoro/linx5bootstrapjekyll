AmazonEC2API
============

The AmazonEC2API function allows you to make calls to the Amazon EC2 web
services.

Properties
----------

-  #### Action

    The desired action to perform on the Amazon EC2 service.

-  #### API access key

    Your unique API Access Key provided by Amazon.

-  #### API secret key

    Your unique API Secret Key provided by Amazon.

-  #### Region name

    The region name where the desired resources reside.

-  #### Parameters

    The relevant properties are displayed in the parameters category,
    depending on which Action is selected. Please see the action
    parameters in the Actions section below.

AmazonEC2API Output
-------------------

The AmazonEC2API function will return different output values depending
on what Action is chosen. Please see the outputs of the actions in the
Action list below.

Actions
-------

-  #### AddNameTag

    Adds a Name Tag to an instance or image.

    ##### Parameters:

    Name - The value of the tag to assign  
     ResourcesID - The ID of the image/instance

    ##### Output:

    None

-  #### CreateImage

    Creates an image from a running instance. This action only starts
    the image creation and will not wait for the image to be created. If
    you wish to terminate the instance, please use the
    WaitUntilImageIsAvailable action on the image before terminating the
    instance.

    ##### Parameters:

    InstanceID - The ID of the running instance  
     ImageName - The name the image should be saved as

    ##### Output(String):

    The ID of the new image

-  #### DeleteNameTag

    Removes the name tag from an instance or image.

    ##### Parameters:

    ResourceId - The ID of the instance or image to remove the name tag
    from

    ##### Output:

    None

-  #### DeleteSnapshot

    Deletes a snapshot.

    ##### Parameters:

    SnapshotId - The ID of the snapshot to delete

    ##### Output:

    None

-  #### DeregisterImage

    Deregisters an image.

    ##### Parameters:

    ImageId - The ID of the image to deregister

    ##### Output:

    None

-  #### GetImageByImageId

    Retrieves an image by a referenced ImageId.

    ##### Parameters:

    ImageId - The ID of the image to retrieve

    ##### Output(Complex Object):

    Image

-  #### GetImageByName

    Retrieves an image by a referenced name.

    ##### Parameters:

    ImageName - The Name of the image to retrieve

    ##### Output(Complex Object):

    Image

-  #### GetImageByNameTag

    Retrieves an image by a referenced name tag.

    ##### Parameters:

    ImageNameTag - The name tag value of the image to retrieve

    ##### Output(Complex Object):

    Image

-  #### GetImages

    Retrieves all images.

    ##### Parameters:

    None

    ##### Output(List):

    A list of Images

- #### GetInstanceById

    Retrieves an instance by a referenced InstanceId.

    ##### Parameters:

    InstanceId - The ID of the instance to retrieve

    ##### Output(Complex Object):

    Instance

- #### GetInstanceByNameTag

    Retrieves an instance by a referenced name tag value.

    ##### Parameters:

    InstanceNameTag - The name tag value of the instance to retrieve

    ##### Output(Complex Object):

    Instance

- #### GetInstanceStatus

    Retrieves the status of an instance by a referenced InstanceId.

    ##### Parameters:

    InstanceId - The ID of the instance to retrieve

    ##### Output(String):

    The status of the instance

- #### GetSnapshots

    Retrieves all snapshots.

    ##### Parameters:

    None

    ##### Output(List):

    A list of Snapshots

- #### GetVolumes

    Retrieves all Volumes.

    ##### Parameters:

    None

    ##### Output(List):

    A list of Volumes

- #### RunInstance

    Runs an instance of an image.

    ##### Parameters:

    ImageId - The ID of the image to run  
     KeyPairName - The name of the keypair to use  
     SubnetId - The Subnet Id supplied by Amazon  
     InstanceType - The type/size of the instance to run  
     VpcSecurityGroupId - The VPC security group ID the instance should
    run under

    ##### Output(Complex Object):

    Instance

- #### TerminateInstance

    Terminates a running instance.

    ##### Parameters:

    InstanceId - The ID of the instance to terminate

    ##### Output:

    None

- #### WaitUntilImageIsAvailable

    Pauses execution until the specified image has a status of
    available.

    ##### Parameters:

    ImageId - The ID of the image to wait for  
     TimeoutInSeconds - The maximum number of seconds to wait for the
    image to be available  
     CheckIntervalInSeconds - The number of seconds to wait between
    querying the state of the image

    ##### Output:

    None

- #### WaitUntilInstanceIsReady

    Pauses execution until the specified instance has a status of
    ready.

    ##### Parameters:

    InstanceId - The ID of the instance to wait for  
     TimeoutInSeconds - The maximum number of seconds to wait for the
    instance to be ready  
     CheckIntervalInSeconds - The number of seconds to wait between
    querying the state of the instance

    ##### Output:

    None


