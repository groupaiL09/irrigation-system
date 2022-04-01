<?php
    $con = mysqli_connect('localhost', 'root', '', 'unity_app');
    // check connection to DB
    if (mysqli_connect_errno()){
        echo "1";   //error code #1 = connection failed
        exit();
    }   

    $farm_id = $_POST["farm_id"];
    $location = $_POST["location"];
    $description = $_POST["description"];

    $tempQuery = "UPDATE farms SET location = '" . $location . "', description = '" . $description . "' WHERE farm_id = '" . $farm_id . "';";
    $tempCheck = mysqli_query($con, $tempQuery) or die("");

    // echo "Rick Nguyen";
    //
        // echo mysqli_num_rows($tempCheck);

        // echo $user_idCheck;
        // echo mysqli_num_rows($user_idCheck);

        // if (mysqli_num_rows($namecheck) != 1){
        //     //at least 1 username matches the username being requested
        //     echo "No matching Username"; //error code: No matching Username
        //     exit();
        // }
        // else{
        //     //get login info from query
        //     $existinginfo = mysqli_fetch_assoc($namecheck);
        //     $salt = $existinginfo["salt"];
        //     $hash = $existinginfo["hash"];
            
        //     $loginhash = crypt($password, $salt);
        //     if ($hash != $loginhash){
        //         echo "Incorrect password"; //error code: password does not hash to match table
        //         exit();
        //     }
        // }

        // echo("0");

?>