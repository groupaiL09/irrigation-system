<?php
    $con = mysqli_connect('localhost', 'root', '', 'unity_app'); 

    $emailMaxLength = 100;



    //check connection to DB
    if (mysqli_connect_errno()){
        echo "connection failed";//error code: connection failed
        exit();
    }    

    $email = strtolower($_POST["email"]);
    $username = $_POST["username"];
    $password1 = $_POST["password1"];
    $password2 = $_POST["password2"];

	//Validate email
	if(preg_match('/\s/', $email)){
		echo "Email can't have spaces";
        exit();
	}
    elseif (!validate_email_address($email)){
		echo "Invalid email";
        exit();
	}
    elseif (strlen($email) > $emailMaxLength){
        echo "Email is too long, must be equal or under " . strval($emailMaxLength) . " characters";
        exit();
    }

    //Validate password
	if($password1 != $password2){
		echo "Passwords do not match";
        exit();
	}
    elseif (preg_match('/\s/', $password1)){
		echo "Password can't have spaces";
        exit();
    }
    elseif (!preg_match('/[A-Za-z]/', $password1) || !preg_match('/[0-9]/', $password1)){
        echo "Password must contain at least 1 letter and 1 number";
        exit();
    }
    
    //check if username or email exists
    $namecheckquery = "SELECT username FROM users WHERE username = '" . $username . "';";
    $namecheck = mysqli_query($con, $namecheckquery) or die("Name check query failed"); // error code: name check query failed

    $emailcheckquery = "SELECT email FROM users WHERE email = '" . $email . "';";
    $emailcheck = mysqli_query($con, $emailcheckquery) or die("email check query failed"); // error code: email check query failed

    if (mysqli_num_rows($namecheck) > 0){
        //at least 1 username matches the username being requested
        echo "UserName has already existed"; //error code: name exists cannot register
        exit();
    }
    elseif (mysqli_num_rows($emailcheck) > 0){
        echo "Email has already existed"; //error code: name exists cannot register
        exit();
    }
    else{
        //add user to DB table
        $salt = "\$5\$rounds=5000\$" . "steamedhams" . $username . "\$";
        $hash = crypt($password1, $salt);
        $insertuserquery = "INSERT INTO users (username, email, hash, salt) VALUES ('" . $username . "', '" . $email . "', '" . $hash . "', '" . $salt . "');";
        mysqli_query($con, $insertuserquery) or die("Insert user query failed"); //error code: insert query failed
    }

    function validate_email_address($email) {
		return preg_match('/^([a-z0-9!#$%&\'*+-\/=?^_`{|}~.]+@[a-z0-9.-]+\.[a-z0-9]+)$/i', $email);
	}

    echo("0");

?>