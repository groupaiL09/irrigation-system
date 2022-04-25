<?php
    include_once("connection.php");

    //check connection to DB
    if (mysqli_connect_errno()){
        echo "connection failed";//error code #1 = connection failed
        exit();
    }    

    $farmId = $_POST["farmId"];

    /*
    //check if username exists
    $namecheckquery = "SELECT username, salt, hash FROM users WHERE username = '" . $username . "';";

    $namecheck = mysqli_query($con, $namecheckquery) or die("Name check query failed"); // error code: name check query failed

    if (mysqli_num_rows($namecheck) != 1){
        //at least 1 username matches the username being requested
        echo "No matching Username"; //error code: No matching Username
        exit();
    }
    else{
        //get login info from query
        $existinginfo = mysqli_fetch_assoc($namecheck);
        $salt = $existinginfo["salt"];
        $hash = $existinginfo["hash"];
        
        $loginhash = crypt($password, $salt);
        if ($hash != $loginhash){
            echo "Incorrect password"; //error code: password does not hash to match table
            exit();
        }
    }*/
    
    $deleteFarmquery = "DELETE FROM farms WHERE farm_id = '". $farmId . "';";
    mysqli_query($con, $deleteFarmquery) or die("Delete farm query failed");

    echo "0";
    

?>

